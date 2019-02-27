using System;
using System.Collections.Generic;

namespace Hsp.PsOsc
{

  internal class PlayStateHandler : MessageHandlerBase
  {

    public PlayStateHandler() : base("^/play$")
    {
    }


    public override void Process(Dictionary<string, string> pathArguments, object[] values)
    {
      var value = (Single) values[0];
      Engine.Instance.Playing = value == 1;
    }

  }

}
