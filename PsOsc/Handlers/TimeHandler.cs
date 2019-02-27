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
      Engine.Instance.CurrentTime = (float)values[0];
    }

  }

}