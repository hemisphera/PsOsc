using System;
using System.Collections.Generic;
using System.IO;
using Hsp.PsOsc.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsOsc.Extensibility.Json;

namespace Hsp.PsOsc
{

  public class SongConfiguration : IJsonSerializable
  {

    public string SongName { get; set; }

    public List<ISongEvent> Events { get; }


    public SongConfiguration()
    {
      Events = new List<ISongEvent>();
    }


    private static ISongEvent ReadEvent(JObject obj, JsonSerializer serializer)
    {
      var typeName = obj.Value<string>("ClrType");
      var type = Type.GetType(typeName);
      if (type == null) return null;

      var typedItem = (ISongEvent)Activator.CreateInstance(type);
      typedItem.TriggerTime = obj.Value<float>();
      typedItem.VoiceGroup = obj.Value<string>();

      var subReader = (obj.GetValue("Data") as JObject)?.CreateReader();
      if (subReader == null) return null;

      typedItem.ReadJson(subReader, serializer);
      return typedItem;
    }


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      var jo = JObject.Load(jr);
      SongName = jo.Value<string>(nameof(SongName));

      if (jo.GetValue(nameof(Events)) is JArray events)
      {
        Events.Clear();
        foreach (var item in events)
        {
          if (!(item is JObject itemObject)) continue;
          try
          {
            var @event = ReadEvent(itemObject, serializer);
            if (@event != null)
              Events.Add(@event);
          }
          catch
          {
            // ignore
          }
        }
      }
    }

    public void WriteJson(JsonWriter jw, JsonSerializer serializer)
    {
      jw.WriteStartObject();

      jw.WritePropertyName(nameof(SongName));
      jw.WriteValue(SongName);

      jw.WritePropertyName(nameof(Events));
      jw.WriteStartArray();

      foreach (var item in Events)
      {
        jw.WriteStartObject();

        jw.WritePropertyName("ClrType");
        jw.WriteValue(item.GetType().FullName);

        jw.WritePropertyName(nameof(item.TriggerTime));
        jw.WriteValue(item.TriggerTime);

        jw.WritePropertyName(nameof(item.VoiceGroup));
        jw.WriteValue(item.VoiceGroup);

        jw.WritePropertyName("Data");
        item.WriteJson(jw, serializer);

        jw.WriteEndObject();
      }

      jw.WriteEndArray();

      jw.WriteEndObject();
    }


    public void LoadFromFile(string filename)
    {
      using (var sr = File.OpenText(filename))
      using (var jr = new JsonTextReader(sr))
        ReadJson(jr, JsonSerializer.Create(SerializerSettings.Instance));
    }

    public void WriteToFile(string filename)
    {
      var s = JsonSerializer.Create(SerializerSettings.Instance);

      using (var sw = File.CreateText(filename))
      using (var jw = new JsonTextWriter(sw))
        s.Serialize(jw, this);
    }

  }

}