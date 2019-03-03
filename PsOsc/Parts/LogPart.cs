using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using eos.Mvvm.Core;
using Rug.Osc;

namespace Hsp.PsOsc.Parts
{

  public class LogPart : AsyncItemsViewModelBase<LogEntry>
  {

    public UiCommand ClearCommand => GetAutoFieldValue(new UiCommand
    {
      Title = "Remove",
      Image = "Remove",
      ExecuteFunction = async parameter =>
      {
        Items.Clear();
      }
    });


    public bool LogAllMessages
    {
      get => GetAutoFieldValue<bool>();
      set
      {
        SetAutoFieldValue(value);
        if (value)
          Engine.Instance.Interface.MessageReceived += InterfaceOnMessageReceived;
        else
          Engine.Instance.Interface.MessageReceived -= InterfaceOnMessageReceived;
      }
    }


    public LogPart() : base(new ObservableCollection<LogEntry>())
    {
      Engine.Instance.OnLogEntryReceived += InstanceOnOnLogEntryReceived;
    }


    private void InterfaceOnMessageReceived(object sender, OscMessage e)
    {
      Write(e);
    }

    private void InstanceOnOnLogEntryReceived(object sender, string e)
    {
      Write(e);
    }


    protected override async Task<IEnumerable<LogEntry>> GetItems()
    {
      return await Task.FromResult(Items.ToArray());
    }


    public void Write(string msg)
    {
      DispatchAsync(() =>
      {
        Items.Insert(0, new LogEntry(msg));
        while (Items.Count > 100)
          Items.RemoveAt(100);
      });
    }

    public void Write(OscMessage msg)
    {
      Write($"{msg}");
    }


    public override void Dispose()
    {
      Engine.Instance.OnLogEntryReceived -= InstanceOnOnLogEntryReceived;
      base.Dispose();
    }

  }

}
