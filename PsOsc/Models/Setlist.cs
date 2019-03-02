using System.Collections.Generic;
using System.IO;
using eos.Mvvm.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsOsc.Extensibility.Json;

namespace Hsp.PsOsc
{

  public class Setlist : IJsonSerializable
  {

    public string Name { get; set; }

    public List<SetlistItem> Songs { get; }


    public Setlist()
    {
      Songs = new List<SetlistItem>();
    }


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      var jo = JObject.Load(jr);
      Name = jo.Value<string>(nameof(Name));

      if (!(jo.GetValue(nameof(Songs)) is JArray songsArray)) return;
      
      Songs.Clear();
      foreach (var token in songsArray)
      {
        var song = new SetlistItem();
        song.ReadJson(token.CreateReader(), serializer);
        Songs.Add(song);
      }
    }

    public void WriteJson(JsonWriter jw, JsonSerializer serializer)
    {
      jw.WriteStartObject();

      jw.WritePropertyName(nameof(Name));
      jw.WriteValue(Name);

      jw.WritePropertyName(nameof(Songs));
      serializer.Serialize(jw, Songs);

      jw.WriteEndObject();
    }


    public void LoadFromFile(string path)
    {
      using (var fs = File.OpenText(path))
      using (var jr = new JsonTextReader(fs))
        ReadJson(jr, JsonSerializer.Create(SerializerSettings.Instance));
    }

    public void SaveToFile(string path)
    {
      using (var fs = File.CreateText(path))
      using (var jw = new JsonTextWriter(fs))
        WriteJson(jw, JsonSerializer.Create(SerializerSettings.Instance));
    }

  }

}