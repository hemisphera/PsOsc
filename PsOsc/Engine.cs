using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using eos.Mvvm.Core;
using Hsp.PsOsc.Extensibility;
using Rug.Osc;

namespace Hsp.PsOsc
{

  public class Engine : ViewModelBase, IPsOscEngine
  {

    private const int TrackCount = 64;

    private const int RegionCount = 64;


    private OscReceiver Receiver { get; set; }

    private OscSender Sender { get; set; }


    private static Engine _instance;

    public static Engine Instance => _instance ?? (_instance = new Engine());


    private List<RegionSlot> RegionsInternal { get; }

    private List<TrackSlot> TracksInternal { get; }


    public IReadOnlyList<IRegion> Regions => RegionsInternal.AsReadOnly();

    public IReadOnlyList<ITrack> Tracks => TracksInternal.AsReadOnly();


    public List<SongConfiguration> SongConfigurations { get; }

    public List<MessageHandlerBase> Handlers { get; }

    public IRegion CurrentRegion
    {
      get => GetAutoFieldValue<IRegion>();
      set
      {
        SetAutoFieldValue(value);
        CurrentRegionChanged?.Invoke(this, value);
      }
    }

    public float CurrentTime
    {
      get => GetAutoFieldValue<float>();
      set
      {
        SetAutoFieldValue(value);
        (CurrentRegion as RegionSlot)?.UpdateTime(value);
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

      Handlers = new List<MessageHandlerBase>
      {
        new RegionHandler(),
        new TimeHandler(),
        new LastRegionHandler(),
        new TrackHandler()
      };

      SongConfigurations = new List<SongConfiguration>();

      Connect();
    }


    private void Connect()
    {
      Receiver = new OscReceiver(IPAddress.Any, Properties.Settings.Default.LocalPort);
      Receiver.Connect();
      Receiver.Close();
      Receiver.Connect();

      Sender = new OscSender(IPAddress.Parse(Properties.Settings.Default.DawHostname), 0, Properties.Settings.Default.DawPort);
      Sender.Connect();
      Sender.Close();
      Sender.Connect();
      
      Task.Run(OscReceiverTaskHandler);

      SendOscMessage("/device/region/count", 0);
      SendOscMessage("/device/region/count", RegionCount);
      SendOscMessage("/device/track/count", 0);
      SendOscMessage("/device/track/count", TrackCount);
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
      Sender.Send(new OscMessage(address, arguments));
    }


    public TrackSlot GetTrack(int index)
    {
      return TracksInternal.FirstOrDefault(t => t.Index == index);
    }

    public RegionSlot GetRegion(int index)
    {
      return RegionsInternal.FirstOrDefault(t => t.Index == index);
    }

  }

}
