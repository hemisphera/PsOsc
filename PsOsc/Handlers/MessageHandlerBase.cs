using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Hsp.PsOsc
{

  public abstract class MessageHandlerBase
  {

    public Regex Regex { get; protected set; }


    protected MessageHandlerBase(string pattern)
    {
      Regex = new Regex(pattern, RegexOptions.Compiled);
    }


    public abstract void Process(Dictionary<string, string> pathArguments, object[] values);

  }

}
