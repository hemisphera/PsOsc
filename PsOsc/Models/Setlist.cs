using System.Collections.Generic;
using eos.Mvvm.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsOsc.Extensibility.Json;

namespace Hsp.PsOsc
{

  public class Setlist : IJsonSerializable
  {

    public string Name { get; set; }

    public List<string> Songs { get; }


    public Setlist()
    {
      Songs = new List<string>();
    }


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      var jo = JObject.Load(jr);
      Name = jo.Value<string>(nameof(Name));

      if (jo.GetValue(nameof(Songs)) is JArray songsArray)
      {
        Songs.Clear();
        foreach (var token in songsArray)
          Songs.Add(token.Value<string>());
      }
    }

    public void WriteJson(JsonWriter jw, JsonSerializer serializer)
    {
      jw.WriteStartObject();

      jw.WritePropertyName(nameof(Name));
      jw.WriteValue(Name);

      jw.WritePropertyName(nameof(Songs));
      jw.WriteValue(Songs.ToArray());

      jw.WriteEndObject();
    }

  }

}