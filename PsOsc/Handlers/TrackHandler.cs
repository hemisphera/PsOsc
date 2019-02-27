using System;
using System.Collections.Generic;

namespace Hsp.PsOsc
{

  internal class TrackHandler : MessageHandlerBase
  {

    public TrackHandler() : base("^/track/(?<index>[0-9]+)/(?<property>.*?)$")
    {
    }

    public override void Process(Dictionary<string, string> pathArguments, object[] values)
    {
      var trackIndex = int.Parse(pathArguments["index"]);

      var track = Engine.Instance.GetTrack(trackIndex);
      if (pathArguments["property"] == "name")
        track.Name = (string) values[0];
      if (pathArguments["property"] == "mute")
        track.IsMuted = (Single) values[0] == 1;
      if (pathArguments["property"] == "solo")
        track.IsSoloed = (Single) values[0] == 1;
      if (pathArguments["property"] == "vu")
        track.Volume = (float) values[0];
      if (pathArguments["property"] == "number/str")
      {
        var trackIdStr = (string) values[0];
        track.Id = String.IsNullOrEmpty(trackIdStr) ? (int?) null : int.Parse(trackIdStr);
      }
    }

  }

}
