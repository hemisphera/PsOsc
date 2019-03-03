using System;

namespace Hsp.PsOsc
{

  public class LogEntry
  {

    public DateTime Timestamp { get; }
    
    public string Message { get; }


    public LogEntry(string message)
    {
      Message = message;
      Timestamp = DateTime.Now;
    }

  }

}
