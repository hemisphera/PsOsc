using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsp.PsOsc;

namespace PsOsc.Test
{
  class Program
  {

    static void Main(string[] args)
    {

      var ifc = new OscInterface();
      ifc.Connect();

      Console.ReadLine();

    }

  }

}