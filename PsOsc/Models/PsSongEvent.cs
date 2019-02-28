using System;
using System.IO;
using System.Management.Automation;
using System.Net;
using Hsp.PowerShell.Utility;
using Hsp.PsOsc.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hsp.PsOsc
{

  public class PsSongEvent : ISongEvent
  {

    public float TriggerTime { get; set; }
    
    public string VoiceGroup { get; set; }

    public string ScriptFilename { get; set; }


    public void ReadJson(JsonReader jr, JsonSerializer serializer)
    {
      var jo = JObject.Load(jr);
      ScriptFilename = jo.Value<string>(nameof(ScriptFilename));
    }

    public void WriteJson(JsonWriter jw, JsonSerializer serializer)
    {
      jw.WriteStartObject();

      jw.WritePropertyName(nameof(ScriptFilename));
      jw.WriteValue(ScriptFilename ?? "");

      jw.WriteEndObject();
    }

    public void Run(IPsOscEngine engine)
    {
      var scriptData = File.ReadAllText(ScriptFilename);
      var sb = ScriptBlock.Create(scriptData);
      sb.Invoke(this, engine);
    }

  }

}