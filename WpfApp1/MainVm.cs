using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Data;
using eos.Mvvm.Core;
using Rug.Osc;

namespace WpfApp1
{

  public class MainVm : ViewModelBase
  {

    private static MainVm _instance;

    public static MainVm Instance => _instance ?? (_instance = new MainVm());


    private static readonly object SyncRoot = new object();




    private OscReceiver Receiver { get; }

    private OscSender Sender { get; }


    public float Time
    {
      get => GetAutoFieldValue<float>();
      set => SetAutoFieldValue(value);
    }

    public SongVm SelectedSong
    {
      get => GetAutoFieldValue<SongVm>();
      set
      {
        SetAutoFieldValue(value);
        Sender.Send(new OscMessage("/time", value.StartTime));
      }
    }

    public SongVm CurrentSong
    {
      get => GetAutoFieldValue<SongVm>();
      set
      {
        var oldSong = CurrentSong;
        oldSong?.Stop();
        SetAutoFieldValue(value);
      }
    }

    public List<MessageHandlerBase> Handlers { get; }

    public UiCommand PlayCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter => { Sender.Send(new OscMessage("/play")); }
    });

    public UiCommand StopCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter => { Sender.Send(new OscMessage("/stop")); }
    });

    public UiCommand PauseCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter => { Sender.Send(new OscMessage("/pause")); }
    });

    public ObservableCollection<SongVm> Songs { get; }
    

    private MainVm()
    {
      UiSettings.Mediator = new Mediator();

      Handlers = new List<MessageHandlerBase>
      {
        new MarkerHandler(),
        new TimeHandler(),
        new LastMarkerHandler()
      };

      Songs = new ObservableCollection<SongVm>();
      BindingOperations.EnableCollectionSynchronization(Songs, SyncRoot);

      Receiver = new OscReceiver(IPAddress.Any, 9000);
      Receiver.Connect();

      var ip = Dns.GetHostEntry("10.0.0.50").AddressList.FirstOrDefault();
      Sender = new OscSender(IPAddress.Parse("10.0.0.50"), 0, 9010);
      Sender.Connect();

      Task.Run(OscReceiverTaskHandler);
      Sender.Send(new OscMessage("/action", 41743));
    }


    private Task OscReceiverTaskHandler()
    {
      while (true)
      {
        var packet = Receiver.Receive();
        var messages = packet is OscBundle bundle ? bundle.Cast<OscMessage>().ToArray() : new[] { packet as OscMessage };
        foreach (var message in messages)
        foreach (var handler in Handlers)
        {
          var m = handler.Regex.Match(message.Address);
          if (!m.Success) continue;

          var groupNames = handler.Regex.GetGroupNames();
          var args = groupNames.ToDictionary(
            name => name,
            name => m.Groups[name].Value);
          handler.Process(args, message.ToArray());
        }
      }
    }

  }

}