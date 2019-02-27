using System.Threading.Tasks;
using Hsp.PsOsc.Extensibility;

namespace Hsp.PsOsc
{

  public class TrackSlot : SlotBase, ITrack
  {

    public bool IsMuted
    {
      get => GetAutoFieldValue<bool>();
      set => SetAutoFieldValue(value);
    }

    public bool IsSoloed
    {
      get => GetAutoFieldValue<bool>();
      set => SetAutoFieldValue(value);
    }

    public float Volume
    {
      get => GetAutoFieldValue<float>();
      set => SetAutoFieldValue(value);
    }


    public TrackSlot(int index) : base(index)
    {
    }


    protected override void Update()
    {
      //throw new System.NotImplementedException();
    }

    public async Task ToggleMute()
    {
      Engine.Instance.SendOscMessage($"/track/{Index}/mute", IsMuted ? 0 : 1);
      await Task.CompletedTask;
    }

    public async Task ToggleSolo()
    {
      Engine.Instance.SendOscMessage($"/track/{Index}/solo", IsSoloed ? 0 : 1);
      await Task.CompletedTask;
    }

  }

}
