using Newtonsoft.Json;

namespace PsOsc.Extensibility.Json
{

  public class SerializerSettings : JsonSerializerSettings
  {

    private static SerializerSettings _instance;

    public static SerializerSettings Instance => _instance ?? (_instance = new SerializerSettings());


    private SerializerSettings()
    {
      Converters.Add(new JsonSerializableConverter());
      Formatting = Formatting.Indented;
    }

  }

}