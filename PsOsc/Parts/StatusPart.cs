using eos.Mvvm.Core;

namespace Hsp.PsOsc.Parts
{

  public class StatusPart : ViewModelBase
  {

    public UiCommand ConnectCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter =>
      {
        Connect();
      }
    });

    public UiCommand DisconnectCommand => GetAutoFieldValue(new UiCommand
    {
      ExecuteAction = parameter =>
      {
        Disconnect();
      }
    });


    public bool Connected
    {
      get => GetAutoFieldValue<bool>();
      private set => SetAutoFieldValue(value);
    }

    public float Time
    {
      get => GetAutoFieldValue<float>();
      private set => SetAutoFieldValue(value);
    }

    public bool Playing
    {
      get => GetAutoFieldValue<bool>();
      set => SetAutoFieldValue(value);
    }

    public RegionSlot CurrentRegion
    {
      get => GetAutoFieldValue<RegionSlot>();
      private set => SetAutoFieldValue(value);
    }


    public string DawHostname
    {
      get => Properties.Settings.Default.DawHostname;
      set
      {
        Properties.Settings.Default.DawHostname = value;
        Properties.Settings.Default.Save();
        RaisePropertyChanged();
      }
    }

    public int HostPort
    {
      get => Properties.Settings.Default.DawPort;
      set
      {
        Properties.Settings.Default.DawPort = value;
        Properties.Settings.Default.Save();
        RaisePropertyChanged();
      }
    }

    public int LocalPort
    {
      get => Properties.Settings.Default.LocalPort;
      set
      {
        Properties.Settings.Default.LocalPort = value;
        Properties.Settings.Default.Save();
        RaisePropertyChanged();
      }
    }


    public StatusPart()
    {
      Engine.Instance.CurrentRegionChanged += (s, e) => { CurrentRegion = e as RegionSlot; };
      Engine.Instance.TimeChanged += (s, e) => { Time = e; };
    }

    
    private void Disconnect()
    {
      Engine.Instance.Interface.Disconnect();
      Connected = false;
    }

    private void Connect()
    {
      var ifc = Engine.Instance.Interface;
      Engine.Instance.Interface.Connect();
      Connected = true;
    }

  }

}
