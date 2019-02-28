using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsp.PsOsc.Extensibility;
using Newtonsoft.Json.Linq;

namespace Hsp.PsOsc
{

  public class MidiSongEvent : SongEventBase
  {

    public byte Channel { get; set; }

    public byte Status { get; set; }

    public byte Data1 { get; set; }

    public byte Data2 { get; set; }


    public override void Run(IPsOscEngine engine)
    {
      throw new NotImplementedException();
    }

    public override JObject GetDataAsObject()
    {
      return new JObject
      {
        {nameof(Channel), Channel},
        {nameof(Status), Status},
        {nameof(Data1), Data1},
        {nameof(Data2), Data2},
      };
    }

    public override void SetDataFromObject(JObject jo)
    {
      Channel = jo.Value<byte>(nameof(Channel));
      Status = jo.Value<byte>(nameof(Status));
      Data1 = jo.Value<byte>(nameof(Data1));
      Data2 = jo.Value<byte>(nameof(Data2));
    }

  }

}
