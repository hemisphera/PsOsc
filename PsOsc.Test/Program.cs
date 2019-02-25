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
      var cfg = new SongConfiguration
      {
        SongName = "Blah",
        Events =
        {
          new OscSongEvent
          {
            Address = "/action/41737",
            Data = ""
          }
        }
      };
      cfg.WriteToFile(@"c:\temp\test.json");
    }

  }

}