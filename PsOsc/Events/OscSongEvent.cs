using System;
using Hsp.PsOsc.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hsp.PsOsc
{

  public class OscSongEvent : SongEventBase
  {

    public string Address { get; set; }

    public string Data { get; set; }

    public string DataType { get; set; }


    public override JObject GetDataAsObject()
    {
      var jo = new JObject
      {
        { nameof(Address), Address },
        { nameof(Data), Data },
        { nameof(DataType), DataType }
      };
      return jo;
    }

    public override void SetDataFromObject(JObject obj)
    {
      Address = obj.Value<string>(nameof(Address));
      Data = obj.Value<string>(nameof(Data));
      DataType = obj.Value<string>(nameof(DataType));
    }


    public override void Run(IPsOscEngine engine)
    {
      if (String.IsNullOrEmpty(Data))
        engine.SendOscMessage(Address);
      else
        engine.SendOscMessage(Address, ConvertValue());
    }

    private object ConvertValue()
    {
      if (String.IsNullOrEmpty(DataType))
        DataType = "s";
      var typeChar = DataType.ToLowerInvariant()[0];
      switch (typeChar)
      {
        case 'f': return float.Parse(Data);
        case 'b': return (Single) (bool.Parse(Data) ? 1 : 0);
        default: return Data;
      }
    }

  }

}