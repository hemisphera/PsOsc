using System.Linq;
using eos.Mvvm.Core;

namespace Hsp.PsOsc
{

  public class Song : ViewModelBase
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

    public int? Id
    {
      get => GetAutoFieldValue<int?>();
      set
      {
        SetAutoFieldValue(value);
        if (value == null)
        {
          Name = null;
          StartTime = 0;
          Duration = null;
          Position = 0;
        }
      }
    }

    public int? Sequence
    {
      get => GetAutoFieldValue<int?>();
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

    public float? Duration
    {
      get => GetAutoFieldValue<float?>();
      set => SetAutoFieldValue(value);
    }

    public float Percentage
    {
      get => GetAutoFieldValue<float>();
      private set => SetAutoFieldValue(value);
    }


    private float? CalcDuration()
    {
      if (Id == null) return null;
      var nextSong = MainVm.Instance.Songs.FirstOrDefault(s => s.Index == Index + 1);
      if (nextSong?.Id == null) return null;
      return nextSong.StartTime - StartTime;
    }

    public void UpdateTime()
    {
      var newPosition = MainVm.Instance.Time - StartTime;
      if (newPosition < 0) newPosition = 0;
      Position = newPosition;

      if (Duration == null)
        Percentage = 0;
      else
        Percentage = Position / Duration.Value;
    }

    public void Stop()
    {
      Position = 0;
      Percentage = 0;
    }

    public void Recalculate()
    {
      Duration = CalcDuration();
      var prevSong = MainVm.Instance.Songs.FirstOrDefault(s => s.Index == Index - 1);
      prevSong?.Recalculate();
    }
  }

}