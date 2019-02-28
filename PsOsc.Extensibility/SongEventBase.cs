using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hsp.PsOsc.Extensibility
{

  public abstract class SongEventBase
  {

    public float TriggerTime { get; set; }

    public int VoiceGroup { get; set; }

    public bool Program { get; set; }


    public abstract void Run(IPsOscEngine engine);


    public abstract JObject GetDataAsObject();

    public abstract void SetDataFromObject(JObject jr);

  }

}
