using System;
using System.Collections.Generic;
using System.Linq;
using Hsp.PsOsc.Extensibility;

namespace Hsp.PsOsc
{

  internal class LastRegionHandler : MessageHandlerBase
  {

    public LastRegionHandler() : base("^/lastregion/(?<property>.*?)$")
    {
    }

    public override void Process(Dictionary<string, string> pathArguments, object[] values)
    {
      if (pathArguments["property"] != "number/str") return;
      
      var regionIdStr = (string) values[0];
      Engine.Instance.CurrentRegion = FindRegion(regionIdStr);
    }

    private static IRegion FindRegion(string id)
    {
      if (String.IsNullOrEmpty(id)) return null;

      var regionId = int.Parse(id);
      return Engine.Instance.Regions.FirstOrDefault(m => m.Id.Equals(regionId));
    }

  }

}