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


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      var jo = JObject.Load(jr);
      Data = jo.Value<string>(nameof(Data));
      Address = jo.Value<string>(nameof(Address));
    }

    public void WriteJson(JsonWriter jw, JsonSerializer serializer)
    {
      jw.WriteStartObject();

      jw.WritePropertyName(nameof(Address));
      jw.WriteValue(Address);

      jw.WritePropertyName(nameof(Data));
      jw.WriteValue(Data);

      jw.WriteEndObject();
    }


    public void Run(IPsOscEngine engine)
    {
      engine.SendOscMessage(Address, Data);
    }

  }

}