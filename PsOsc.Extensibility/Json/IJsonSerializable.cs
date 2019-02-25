using Newtonsoft.Json;

namespace PsOsc.Extensibility.Json
{

  public interface IJsonSerializable
  {

    void ReadJson(JsonReader jr, JsonSerializer serializer);
    
    void WriteJson(JsonWriter jw, JsonSerializer serializer);

  }

}