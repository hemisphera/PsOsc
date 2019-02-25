using System;
using System.Collections.Generic;
using System.Linq;

namespace Hsp.PsOsc
{

  internal class LastMarkerHandler : MessageHandlerBase
  {

    public LastMarkerHandler() : base("^/lastmarker/(?<property>.*?)$")
    {
    }

    public override void Process(Dictionary<string, string> pathArguments, object[] values)
    {
      if (pathArguments["property"] != "number/str") return;
      var indexStr = (string) values[0];
      if (String.IsNullOrEmpty(indexStr)) return;

      var index = int.Parse(indexStr);

      var lastMarker = MainVm.Instance.Songs.FirstOrDefault(m => m.Id.Equals(index));
      MainVm.Instance.CurrentSong = lastMarker;
    }

  }

}