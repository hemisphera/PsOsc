using System.IO;
using Hsp.PsOsc.Extensibility;
using Newtonsoft.Json.Linq;

namespace Hsp.PsOsc
{

  public class PsSongEvent : SongEventBase
  {

    public string ScriptFilename { get; set; }


    public override JObject GetDataAsObject()
    {
      return new JObject
      {
        {nameof(ScriptFilename), ScriptFilename}
      };
    }

    public override void SetDataFromObject(JObject obj)
    {
      ScriptFilename = obj.Value<string>(nameof(ScriptFilename));
    }

    public override void Run(IPsOscEngine engine)
    {
      var scriptData = File.ReadAllText(ScriptFilename);
      Engine.Instance.PowerShell.StartPipeline();
      Engine.Instance.PowerShell.RegisterVariable("PsOscEvent", this);
      Engine.Instance.PowerShell.AddScript(scriptData);
      Engine.Instance.PowerShell.Invoke();
    }

  }

}