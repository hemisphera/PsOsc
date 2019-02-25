using System;
using System.Linq;
using Newtonsoft.Json;

namespace PsOsc.Extensibility.Json
{

  class JsonSerializableConverter : JsonConverter
  {

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      (value as IJsonSerializable).WriteJson(writer, serializer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var item = Activator.CreateInstance(objectType) as IJsonSerializable;
      item.ReadJson(reader, serializer);
      return item;
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType.GetInterfaces().Contains(typeof(IJsonSerializable));
    }

  }

}
