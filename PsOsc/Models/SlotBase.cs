using System;
using System.Threading;
using System.Threading.Tasks;
using eos.Mvvm.Core;

namespace Hsp.PsOsc
{

  public abstract class SlotBase : ViewModelBase
  {

    private DateTime UpdateIn { get; set; }

    private Task FindItemTask { get; set; }


    protected TimeSpan TriggerDelay { get; set; }

    protected bool WaitingForUpdate => UpdateIn > DateTime.Now;


    public int Index { get; }

    public int? Id
    {
      get => GetAutoFieldValue<int?>();
      set
      {
        SetAutoFieldValue(value);
        RaisePropertyChanged(nameof(Enabled));
        TriggerUpdate();
      }
    }

    public string Name
    {
      get => GetAutoFieldValue<string>();
      set
      {
        SetAutoFieldValue(value);
        TriggerUpdate();
      }
    }

    public bool Enabled => Id != null;


    protected SlotBase(int index)
    {
      Index = index;
      TriggerDelay = TimeSpan.FromMilliseconds(550);
    }


    protected void TriggerUpdate()
    {
      UpdateIn = DateTime.Now.Add(TriggerDelay);

      if (FindItemTask == null || FindItemTask.IsCompleted)
        FindItemTask = Task.Run(() =>
        {
          while (WaitingForUpdate)
            Thread.Sleep(50);
          Update();
        });
    }

    protected abstract void Update();

  }

}