using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Rug.Osc;

namespace ConsoleApp4
{
  class Program
  {
    static void Main(string[] args)
    {
      Task.Run(() =>
      {
        var r = new Rug.Osc.OscReceiver(IPAddress.Any, 9000);
        r.Connect();
        while (true)
        {
          var m = r.Receive();
          var messages = m is OscBundle bundle ? bundle.Cast<OscMessage>().ToArray() : new[] {m as OscMessage};
          foreach (var message in messages)
          {
            if (message.Address.Contains("marker"))
              Console.WriteLine(message);
          }
        }
      });

      var w = new OscSender(IPAddress.Parse("10.0.0.51"), 0, 9010);
      w.Connect();
      w.Send(new OscMessage("/action", 41743));

      Console.ReadLine();
    }
  }
}
