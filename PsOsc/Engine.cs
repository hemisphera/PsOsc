using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using eos.Mvvm.Core;
using Hsp.PowerShell.Utility;
using Hsp.PsOsc.Extensibility;

namespace Hsp.PsOsc
{

  public class Engine : ViewModelBase, IPsOscEngine, IDisposable
  {

    public const int TrackCount = 64;

    public const int RegionCount = 64;



    private static Engine _instance;

    public static Engine Instance => _instance ?? (_instance = new Engine());


    internal ObservableCollection<RegionSlot> RegionsInternal { get; }

    internal ObservableCollection<TrackSlot> TracksInternal { get; }


    public IReadOnlyList<IRegion> Regions => RegionsInternal;

    public IReadOnlyList<ITrack> Tracks => TracksInternal;


    public OscInterface Interface { get; }

    public PsRunner PowerShell { get; }


    public List<SongConfiguration> SongConfigurations { get; }

    public IRegion CurrentRegion
    {
      get => GetAutoFieldValue<IRegion>();
      set
      {
        var oldRegion = CurrentRegion;
        if (SetAutoFieldValue(value))
          if (oldRegion != null && Playing)
            RegionFinished?.Invoke(this, oldRegion);
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
        TimeChanged?.Invoke(this, value);
        (CurrentRegion as RegionSlot)?.TickAbsolute(value);
      }
    }

    public bool Playing
    {
      get => GetAutoFieldValue<bool>();
      set
      {
        SetAutoFieldValue(value);
        MainVm.Instance.Status.Playing = value;
        if (!value)
          StopSongPlayback();
        BeginSongPlayback();
      }
    }


    public event EventHandler<IRegion> CurrentRegionChanged;
    
    public event EventHandler<IRegion> RegionFinished;
    
    public event EventHandler<float> TimeChanged;

    public event EventHandler<string> OnLogEntryReceived; 


    private Engine()
    {
      RegionsInternal = new ObservableCollection<RegionSlot>();
      for (var i = 0; i < RegionCount; i++)
        RegionsInternal.Add(new RegionSlot(i + 1));

      TracksInternal = new ObservableCollection<TrackSlot>();
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

      PowerShell = InitPowerShell();

      SongConfigurations = new List<SongConfiguration>();
    }


    private PsRunner InitPowerShell()
    {
      var psr = new PsRunner(NullInterface.Instance);
      psr.RegisterVariable("PsOscEngine", this);
      psr.AddCommand("Write-Host")
        .AddParameter("Object", "PsOsc Initialized")
        .Invoke();
      return psr;
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

    
    public void Play(IRegion region)
    {
      Interface.Stop();
      
      // a '/lastregion' is only emitted if we're not already on the target region
      var wait = region != CurrentRegion; 
        
      Interface.GotoRegion(region, wait);
      Interface.Play();
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

    public void WriteLogEntry(string message)
    {
      OnLogEntryReceived?.Invoke(this, message);
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