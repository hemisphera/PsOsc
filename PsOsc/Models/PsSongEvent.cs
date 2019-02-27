using System;
using Hsp.PsOsc.Extensibility;
using Newtonsoft.Json;

namespace Hsp.PsOsc
{

  public class PsSongEvent : ISongEvent
  {
    
    public float TriggerTime { get; set; }
    
    public string VoiceGroup { get; set; }

    public string ScriptFilename { get; set; }


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public void WriteJson(JsonWriter jw, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public void Run(IPsOscEngine engine)
    {
      throw new NotImplementedException();
    }

  }

}