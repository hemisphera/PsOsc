using PsOsc.Extensibility.Json;

namespace Hsp.PsOsc.Extensibility
{

  public interface ISongEvent : IJsonSerializable
  {
    
    float TriggerTime { get; set; }

    string VoiceGroup { get; set; }


    void Run();

  }

}