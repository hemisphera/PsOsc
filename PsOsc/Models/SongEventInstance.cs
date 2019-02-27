using Hsp.PsOsc.Extensibility;

namespace Hsp.PsOsc
{

  public class SongEventInstance
  {

    private ISongEvent Event { get; }

    public bool Triggered { get; }


    public SongEventInstance(ISongEvent @event)
    {
      Event = @event;
    }

  }

}
