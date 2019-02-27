using System;
using System.Collections.Generic;

namespace Hsp.PsOsc.Extensibility
{

  public interface IPsOscEngine
  {

    IReadOnlyList<IRegion> Regions { get; }
    
    IReadOnlyList<ITrack> Tracks { get; }


    IRegion CurrentRegion { get; }
    
    float CurrentTime { get; }


    void SendOscMessage(string address, params object[] arguments);


    event EventHandler<IRegion> CurrentRegionChanged;

  }

}