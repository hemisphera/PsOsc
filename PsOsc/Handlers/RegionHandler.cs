using System;
using System.Collections.Generic;

namespace Hsp.PsOsc
{

  internal class RegionHandler : MessageHandlerBase
  {

    public RegionHandler() : base("^/region/(?<index>[0-9]+)/(?<property>.*?)$")
    {
    }

    public override void Process(Dictionary<string, string> pathArguments, object[] values)
    {
      var regionIndex = int.Parse(pathArguments["index"]);

      var song = Engine.Instance.GetRegion(regionIndex);
      if (pathArguments["property"] == "name")
        song.Name = (string) values[0];
      if (pathArguments["property"] == "time")
        song.StartTime = (float) values[0];
      if (pathArguments["property"] == "length")
        song.Duration = (float) values[0];
      if (pathArguments["property"] == "number/str")
      {
        var numberStr = (string) values[0];
        if (!String.IsNullOrEmpty(numberStr))
          song.Id = int.Parse(numberStr);
        else
          song.Id = null;
      }
    }

  }

}