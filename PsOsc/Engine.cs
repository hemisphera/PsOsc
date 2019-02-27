using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using eos.Mvvm.Core;
using Hsp.PsOsc.Extensibility;

namespace Hsp.PsOsc
{

  public class Engine : ViewModelBase, IPsOscEngine, IDisposable
  {

    public const int TrackCount = 64;

    public const int RegionCount = 64;



    private static Engine _instance;

    public static Engine Instance => _instance ?? (_instance = new Engine());


    public OscInterface Osc { get; }


    private List<RegionSlot> RegionsInternal { get; }

    private List<TrackSlot> TracksInternal { get; }


    public IReadOnlyList<IRegion> Regions => RegionsInternal.AsReadOnly();

    public IReadOnlyList<ITrack> Tracks => TracksInternal.AsReadOnly();


    public OscInterface Interface { get; }

    public List<SongConfiguration> SongConfigurations { get; }

    public IRegion CurrentRegion
    {
      get => GetAutoFieldValue<IRegion>();
      set
      {
        SetAutoFieldValue(value);
        BeginSongPlayback();
        CurrentRegionChanged?.Invoke(this, value);
      }
    }

    public float CurrentTime
    {
      get => GetAutoFieldValue<float>();
      set
      {
        SetAutoFieldValue(value);
        (CurrentRegion as RegionSlot)?.TickAbsolute(value);
      }
    }

    public bool Playing
    {
      get => GetAutoFieldValue<bool>();
      set
      {
        SetAutoFieldValue(value);
        if (!value)
          StopSongPlayback();
        BeginSongPlayback();
      }
    }


    public event EventHandler<IRegion> CurrentRegionChanged;


    private Engine()
    {
      RegionsInternal = new List<RegionSlot>();
      for (var i = 0; i < RegionCount; i++)
        RegionsInternal.Add(new RegionSlot(i + 1));

      TracksInternal = new List<TrackSlot>();
      for (var i = 0; i < RegionCount; i++)
        TracksInternal.Add(new TrackSlot(i + 1));

      Interface = new OscInterface();
      Interface.Handlers.AddRange(
        new MessageHandlerBase[] {
          new RegionHandler(),
          new TimeHandler(),
          new LastRegionHandler(),
          new TrackHandler(),
          new PlayStateHandler()
        });

      SongConfigurations = new List<SongConfiguration>();
    }


    private void BeginSongPlayback()
    {
      if (!Playing) return;
      if (!(CurrentRegion is RegionSlot slot)) return;
      slot.BeginPlayback(CurrentTime);
    }

    private void StopSongPlayback()
    {
      if (Playing) return;
      if (!(CurrentRegion is RegionSlot slot)) return;
      slot.StopPlayback(CurrentTime);
    }


    public void LoadSongs()
    {
      if (!Directory.Exists(Properties.Settings.Default.SongLibraryFolder))
        return;
      var files = Directory.EnumerateFiles(
        Properties.Settings.Default.SongLibraryFolder, 
        "*.json",
        SearchOption.AllDirectories);

      SongConfigurations.Clear();
      foreach (var file in files)
      {
        var config = new SongConfiguration();
        config.LoadFromFile(file);
        SongConfigurations.Add(config);
      }
    }


    public void SendOscMessage(string address, params object[] arguments)
    {
      Interface.Send(address, arguments);
    }


    public TrackSlot GetTrack(int index)
    {
      return TracksInternal.FirstOrDefault(t => t.Index == index);
    }

    public RegionSlot GetRegion(int index)
    {
      return RegionsInternal.FirstOrDefault(t => t.Index == index);
    }


    public void Dispose()
    {
      Interface?.Dispose();
    }

  }

}