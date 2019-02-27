using System;
using Hsp.PsOsc.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hsp.PsOsc
{

  public class OscSongEvent : ISongEvent
  {

    public float TriggerTime { get; set; }

    public string VoiceGroup { get; set; }


    public string Address { get; set; }

    public string Data { get; set; }

    public string DataType { get; set; }


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      var jo = JObject.Load(jr);
      Data = jo.Value<string>(nameof(Data));
      Address = jo.Value<string>(nameof(Address));
      DataType = jo.Value<string>(nameof(DataType));
    }

    public void WriteJson(JsonWriter jw, JsonSerializer serializer)
    {
      jw.WriteStartObject();

      jw.WritePropertyName(nameof(Address));
      jw.WriteValue(Address);

      jw.WritePropertyName(nameof(Data));
      jw.WriteValue(Data);

      if (!String.IsNullOrEmpty(DataType))
      {
        jw.WritePropertyName(nameof(DataType));
        jw.WriteValue(Data);
      }

      jw.WriteEndObject();
    }


    public void Run(IPsOscEngine engine)
    {
      if (String.IsNullOrEmpty(Data))
        engine.SendOscMessage(Address);
      else
        engine.SendOscMessage(Address, ConvertValue());
    }

    private object ConvertValue()
    {
      if (String.IsNullOrEmpty(DataType))
        DataType = "s";
      var typeChar = DataType.ToLowerInvariant()[0];
      switch (typeChar)
      {
        case 'f': return float.Parse(Data);
        case 'b': return (Single) (bool.Parse(Data) ? 1 : 0);
        default: return Data;
      }
    }

  }

}