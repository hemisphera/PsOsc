using Hsp.PsOsc.Extensibility;
using System.Threading.Tasks;

namespace Hsp.PsOsc
{

  public class SongEventInstance
  {

    private ISongEvent Event { get; }

    public bool Triggered { get; private set; }

    public float TriggerTime => Event.TriggerTime;


    public SongEventInstance(ISongEvent @event)
    {
      Event = @event;
    }


    public void Run()
    {
      Task.Run(() => Event.Run(Engine.Instance));
      Triggered = true;
    }

  }

}
