using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using eos.Mvvm.Core;
using Microsoft.Win32;

namespace Hsp.PsOsc
{
  
  class Mediator : MappedViewMediator
  {

    public override bool RequestFile(FileRequestEventArgs args)
    {
      switch (args.Type)
      {

        case FileRequestEventArgs.RequestType.OpenFile:
          var dlg1 = new OpenFileDialog
          {
            Filter = args.Filter,
            Title = args.Title,
          };
          args.Confirmed = dlg1.ShowDialog(Application.Current.MainWindow) == true;
          args.SelectedPath = dlg1.FileName;
          return args.Confirmed;
        
        case FileRequestEventArgs.RequestType.SaveFile:
          var dlg2 = new SaveFileDialog
          {
            Filter = args.Filter,
            Title = args.Title,
          };
          args.Confirmed = dlg2.ShowDialog(Application.Current.MainWindow) == true;
          args.SelectedPath = dlg2.FileName;
          return args.Confirmed;
        
        case FileRequestEventArgs.RequestType.OpenFolder:
          break;
      }

      return false;
    }

    public override void HandleException(Exception ex, string message)
    {
      MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public override void ShowMessage(string message, object context)
    {
      MessageBox.Show(Application.Current.MainWindow, message, "Error", MessageBoxButton.OK);
    }

    public override bool? ShowDialog(string message, object context)
    {
      return MessageBox.Show(Application.Current.MainWindow, message, "Error", MessageBoxButton.OKCancel) == MessageBoxResult.OK;
    }

    public override Window CreateProgressWindow()
    {
      throw new NotImplementedException();
    }

  }

}