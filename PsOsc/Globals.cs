using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{

  public class Engine
  {

    private static Engine _instance;

    public static Engine Instance => _instance ?? (_instance = new Engine());


    public float Time { get; set; }


    public TimeSpan RefreshInterval { get; set; }

    private Task UiRefreshTask { get; set; }


    private Engine()
    {
      UiRefreshTask = Task.Run(() =>
      {
        UiRefreshHandler();
      });
      RefreshInterval = TimeSpan.FromMilliseconds(150);
    }


    private void UiRefreshHandler()
    {
      while (true)
      {
        Thread.Sleep(RefreshInterval);
        MainVm.Instance.Time = Time;
      }
    }

  }

}