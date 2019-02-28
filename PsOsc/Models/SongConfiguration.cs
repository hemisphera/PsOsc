using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hsp.PsOsc.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsOsc.Extensibility.Json;

namespace Hsp.PsOsc
{

  public class SongConfiguration : IJsonSerializable
  {

    public string SongName { get; set; }

    public List<SongEventBase> Events { get; }

    private SongEventInstance NextEvent { get; set; }

    public Queue<SongEventInstance> EventQueue { get; }


    public SongConfiguration()
    {
      Events = new List<SongEventBase>();
      EventQueue = new Queue<SongEventInstance>();
    }


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      var jo = JObject.Load(jr);
      SongName = jo.Value<string>(nameof(SongName));

      if (!(jo.GetValue(nameof(Events)) is JArray events)) return;

      Events.Clear();
      foreach (var joEvent in events.Cast<JObject>())
      {
        try
        {
          var eventType = Type.GetType(joEvent.Value<string>("ClrType"));
          if (eventType == null) continue;
          var eventInstance = (SongEventBase) Activator.CreateInstance(eventType);
          eventInstance.TriggerTime = joEvent.Value<float>(nameof(SongEventBase.TriggerTime));
          eventInstance.VoiceGroup = joEvent.Value<int>(nameof(SongEventBase.VoiceGroup));
          eventInstance.Program = joEvent.Value<bool>(nameof(SongEventBase.Program));
          eventInstance.SetDataFromObject(joEvent["Data"] as JObject);
          Events.Add(eventInstance);
        }
        catch
        {
          // ignore
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
        jw.WriteValue(GetType().FullName);

        jw.WritePropertyName(nameof(item.Program));
        jw.WriteValue(item.Program);

        jw.WritePropertyName(nameof(item.TriggerTime));
        jw.WriteValue(item.TriggerTime);

        jw.WritePropertyName(nameof(item.VoiceGroup));
        jw.WriteValue(item.VoiceGroup);

        jw.WritePropertyName("Data");
        item.GetDataAsObject().WriteTo(jw, serializer.Converters.ToArray());

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


    private void FindNextEvent()
    {
      NextEvent =
        (EventQueue.Count == 0)
        ? null 
        : EventQueue.Peek();
    }


    public void Init(float relativePos)
    {
      EventQueue.Clear();
      foreach (var ev in Events.Where(e => e.TriggerTime >= relativePos))
        EventQueue.Enqueue(new SongEventInstance(ev));
      FindNextEvent();
    }

    public void Tick(float relativePos)
    {
      if (NextEvent == null) return;

      while (NextEvent?.TriggerTime <= relativePos)
      {
        var ev = EventQueue.Dequeue();
        TryRunEvent(ev);
        FindNextEvent();
      }
    }

    private static async Task TryRunEvent(SongEventInstance ev)
    {
      await Task.Run(() =>
      {
        try
        {
          ev.Run();
        }
        catch
        {
          // ignore
        }
      });
    }

  }

}