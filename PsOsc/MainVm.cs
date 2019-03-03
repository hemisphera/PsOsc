using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using eos.Mvvm.Core;
using Hsp.PsOsc.Extensibility;
using Hsp.PsOsc.Infrastructure;
using Hsp.PsOsc.Parts;
using Rug.Osc;

namespace Hsp.PsOsc
{

  public class MainVm : ViewModelBase
  {

    private static MainVm _instance;

    public static MainVm Instance => _instance ?? (_instance = new MainVm());


    private static readonly object SyncRoot = new object();


    public string DawIpAddress
    {
      get => GetAutoFieldValue<string>();
      set => SetAutoFieldValue(value);
    }


    public UiCommand PlayCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter =>
      {
        Engine.Instance.SendOscMessage("/play");
      }
    });

    public UiCommand StopCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter =>
      {
        Engine.Instance.SendOscMessage("/stop");
      }
    });

    public UiCommand PauseCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter =>
      {
        Engine.Instance.SendOscMessage("/pause");
      }
    });

    public UiCommand ToggleMuteCommand => GetAutoFieldValue(new UiCommand
    {
      Title = "Reconnect",
      ExecuteFunction = async parameter =>
      {
        if (parameter is TrackSlot track)
          await track.ToggleMute();
      }
    });

    public UiCommand ToggleSoloCommand => GetAutoFieldValue(new UiCommand
    {
      Title = "Reconnect",
      ExecuteFunction = async parameter =>
      {
        if (parameter is TrackSlot track)
          await track.ToggleSolo();
      }
    });


    public TracksPart Tracks { get; }
    
    public RegionsPart Regions { get; }

    public SetlistPart Setlist { get; }
    
    public StatusPart Status { get; }


    private MainVm()
    {
      UiSettings.Mediator = new Mediator();
      Tracks = new TracksPart();
      Regions = new RegionsPart();
      Setlist = new SetlistPart();
      Status = new StatusPart();

      var args = Environment.GetCommandLineArgs();
      if (args.Length > 1)
        Setlist.LoadFromFile(args[1]);

      Engine.Instance.LoadSongs();
    }


    
    internal static void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(sender is IView view)) return;

      if (e.OldValue is IVieWModel ovm)
        ovm.View = null;
      if (e.NewValue is IVieWModel nvm)
        nvm.View = view;
    }

  }

}