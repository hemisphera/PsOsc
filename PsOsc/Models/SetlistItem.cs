using System;
using System.Linq;
using eos.Mvvm.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PsOsc.Extensibility.Json;

namespace Hsp.PsOsc
{

  public class SetlistItem : ViewModelBase, IJsonSerializable
  {

    public int Sequence
    {
      get => GetAutoFieldValue<int>();
      set => SetAutoFieldValue(value);
    }

    public string Name
    {
      get => GetAutoFieldValue<string>();
      set => SetAutoFieldValue(value);
    }

    public string RegionName
    {
      get => GetAutoFieldValue<string>();
      set => SetAutoFieldValue(value);
    }

    public float PauseBefore
    {
      get => GetAutoFieldValue<float>();
      set => SetAutoFieldValue(value);
    }

    public float PauseAfter
    {
      get => GetAutoFieldValue<float>();
      set => SetAutoFieldValue(value);
    }


    public RegionSlot FindRegion()
    {
      if (String.IsNullOrEmpty(RegionName)) return null;
      return Engine.Instance
        .RegionsInternal
        .FirstOrDefault(r => r.Name?.Equals(RegionName) ?? false);
    }


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      var obj = JObject.ReadFrom(jr);

      Sequence = obj.Value<int>(nameof(Sequence));
      Name = obj.Value<string>(nameof(Name));
      RegionName = obj.Value<string>(nameof(RegionName));
      PauseBefore = obj.Value<float>(nameof(PauseBefore));
      PauseAfter = obj.Value<float>(nameof(PauseAfter));
    }

    public void WriteJson(JsonWriter jw, JsonSerializer serializer)
    {
      jw.WriteStartObject();

      jw.WritePropertyName(nameof(Sequence));
      jw.WriteValue(Sequence);

      jw.WritePropertyName(nameof(Name));
      jw.WriteValue(Name);

      jw.WritePropertyName(nameof(RegionName));
      jw.WriteValue(RegionName);

      jw.WritePropertyName(nameof(PauseAfter));
      jw.WriteValue(PauseAfter);

      jw.WritePropertyName(nameof(PauseBefore));
      jw.WriteValue(PauseBefore);

      jw.WriteEndObject();
    }

  }

}