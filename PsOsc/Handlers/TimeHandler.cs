using System;
using System.Collections.Generic;

namespace Hsp.PsOsc
{

  internal class TimeHandler : MessageHandlerBase
  {

    public TimeHandler() : base("^/time$")
    {
    }

    public override void Process(Dictionary<string, string> pathArguments, object[] values)
    {
      MainVm.Instance.Time = (float)values[0];
      MainVm.Instance.CurrentSong?.UpdateTime();
    }

  }

}