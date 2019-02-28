using Hsp.PsOsc.Extensibility;
using System.Threading.Tasks;

namespace Hsp.PsOsc
{

  public class SongEventInstance
  {

    private SongEventBase Event { get; }

    public bool Triggered { get; private set; }

    public float TriggerTime => Event.TriggerTime;


    public SongEventInstance(SongEventBase ev)
    {
      Event = ev;
    }


    public void Run()
    {
      Task.Run(() => Event.Run(Engine.Instance));
      Triggered = true;
    }

  }

}
