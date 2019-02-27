using System;
using System.Linq;
using eos.Mvvm.Core;
using Hsp.PsOsc.Extensibility;

namespace Hsp.PsOsc
{

  public class RegionSlot : SlotBase, IRegion
  {

    public int? Sequence
    {
      get => GetAutoFieldValue<int?>();
      set => SetAutoFieldValue(value);
    }

    public float StartTime
    {
      get => GetAutoFieldValue<float>();
      set
      {
        if (SetAutoFieldValue(value))
          TriggerUpdate();
      }
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


    public SongConfiguration Configuration { get; private set; }


    public RegionSlot(int index) : base(index)
    {
    }


    public void BeginPlayback(float absolutePos)
    {
      var relativePos = absolutePos - StartTime;
      if (relativePos < 0) relativePos = 0;

      Configuration?.Init(relativePos);
      Tick(relativePos);
    }

    public void StopPlayback(float currentTime)
    {
    }

    public void Tick(float relativePos)
    {
      Position = relativePos;
      Configuration?.Tick(relativePos);

      DispatchAsync(() =>
      {
        if (Duration == null)
          Percentage = 0;
        else
          Percentage = Position / Duration.Value;
      });
    }

    public void TickAbsolute(float value)
    {
      var relativePos = value - StartTime;
      if (relativePos < 0) relativePos = 0;
      Tick(relativePos);
    }

    protected override void Update()
    {
      if (Id != null)
        Configuration = Engine.Instance.SongConfigurations.FirstOrDefault(s => s.SongName.Equals(Name));
      else
      {
        Percentage = 0;
        StartTime = 0;
      }
    }

  }

}