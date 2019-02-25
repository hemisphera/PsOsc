using System;
using System.Linq;
using eos.Mvvm.Core;

namespace WpfApp1
{

  public class SongVm : ViewModelBase
  {

    public int Index
    {
      get => GetAutoFieldValue<int>();
      set
      {
        SetAutoFieldValue(value);
        RaisePropertyChanged(nameof(Duration));
      }
    }

    public int Id
    {
      get => GetAutoFieldValue<int>();
      set => SetAutoFieldValue(value);
    }

    public string Name
    {
      get => GetAutoFieldValue<string>();
      set => SetAutoFieldValue(value);
    }

    public float StartTime
    {
      get => GetAutoFieldValue<float>();
      set => SetAutoFieldValue(value);
    }

    public float Position
    {
      get => GetAutoFieldValue<float>();
      private set => SetAutoFieldValue(value);
    }

    public float Duration
    {
      get
      {
        if (NextSong == null) return 0;
        var duration = NextSong.StartTime - StartTime;
        return duration < 0 ? 0 : duration;
      }
    }


    public float Percentage
    {
      get => GetAutoFieldValue<float>();
      private set => SetAutoFieldValue(value);
    }

    public SongVm NextSong => MainVm.Instance.Songs.FirstOrDefault(s => s.Index == Index + 1);


    public void UpdateTime()
    {
      var newPosition = MainVm.Instance.Time - StartTime;
      if (newPosition < 0) newPosition = 0;
      Position = newPosition;

      if (NextSong == null)
        Percentage = 0;
      else
        Percentage = Position / (NextSong.StartTime - StartTime);
    }

    public void Stop()
    {
      Position = 0;
      Percentage = 0;
    }

  }

}