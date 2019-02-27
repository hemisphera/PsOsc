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
        SetAutoFieldValue(value);
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


    public void UpdateTime(float absolutePos)
    {
      var newPosition = absolutePos - StartTime;
      if (newPosition < 0) newPosition = 0;
      Position = newPosition;

      if (Duration == null)
        Percentage = 0;
      else
        Percentage = Position / Duration.Value;
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
      var previousSong = Engine.Instance.GetRegion(Index - 1);
      previousSong?.Update();
    }

  }

}