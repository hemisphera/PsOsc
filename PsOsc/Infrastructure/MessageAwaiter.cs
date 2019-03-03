using System;
using System.Linq;
using System.Threading;
using Rug.Osc;

namespace Hsp.PsOsc.Infrastructure
{

  internal class MessageAwaiter : IDisposable
  {

    private ManualResetEvent Handle { get; }

    public string Address { get; }

    public object Argument { get; }


    public MessageAwaiter(string address, object arg)
    {
      Address = address;
      Argument = arg;
      Handle = new ManualResetEvent(false);
    }

    public void Wait()
    {
      Wait(TimeSpan.MaxValue);
    }

    public void Wait(TimeSpan timeout)
    {
      Handle.Reset();
      Engine.Instance.Interface.MessageReceived += InterfaceOnMessageReceived;
      if (timeout == TimeSpan.MaxValue)
        Handle.WaitOne();
      else
        Handle.WaitOne(timeout);
    }


    private void InterfaceOnMessageReceived(object sender, OscMessage e)
    {
      if (e.Address != Address) return;
      if (Argument != null && !e.Any(a => a.Equals(Argument))) return;

      Engine.Instance.Interface.MessageReceived -= InterfaceOnMessageReceived;
      Handle.Set();
    }

    public void Dispose()
    {
      Engine.Instance.Interface.MessageReceived -= InterfaceOnMessageReceived;
      Handle?.Dispose();
    }

  }

}