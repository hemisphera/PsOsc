using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1
{

  internal class MarkerHandler : MessageHandlerBase
  {

    public MarkerHandler() : base("^/marker/(?<id>[0-9]+)/(?<property>.*?)$")
    {
    }

    public override void Process(Dictionary<string, string> pathArguments, object[] values)
    {
      var songs = MainVm.Instance.Songs;

      var songIndex = int.Parse(pathArguments["id"]);
      var song = songs.FirstOrDefault(m => m.Index == songIndex);
      if (song == null)
      {
        song = new SongVm
        {
          Index = songIndex
        };
        songs.Add(song);
      }

      if (pathArguments["property"] == "name")
        song.Name = (string) values[0];
      if (pathArguments["property"] == "time")
        song.StartTime = (float) values[0];
      if (pathArguments["property"] == "number/str")
      {
        var numberStr = (string) values[0];
        if (!String.IsNullOrEmpty(numberStr))
          song.Id = int.Parse(numberStr);
        else
        {
          song.Name = "";
          song.StartTime = 0;
          song.Id = 0;
        }
      }
    }

  }

}