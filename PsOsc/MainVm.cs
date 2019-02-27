using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Data;
using eos.Mvvm.Core;
using Hsp.PsOsc.Extensibility;
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


    public UiCommand ReconnectCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter =>
      {
        Engine.Instance.Interface.Disconnect();
        Engine.Instance.Interface.Connect();
      }
    });

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


    public IReadOnlyList<IRegion> Songs => Engine.Instance.Regions;

    public IReadOnlyList<ITrack> Tracks => Engine.Instance.Tracks;


    private MainVm()
    {
      UiSettings.Mediator = new Mediator();
      
      Engine.Instance.LoadSongs();

      BindingOperations.EnableCollectionSynchronization(Songs, SyncRoot);
    }


  }

}