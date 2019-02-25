using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using eos.Mvvm.Core;

namespace Hsp.PsOsc
{
  class Mediator : MappedViewMediator
  {
    public override bool RequestFile(FileRequestEventArgs args)
    {
      throw new NotImplementedException();
    }

    public override void HandleException(Exception ex, string message)
    {
      throw new NotImplementedException();
    }

    public override void ShowMessage(string message, object context)
    {
      throw new NotImplementedException();
    }

    public override bool? ShowDialog(string message, object context)
    {
      throw new NotImplementedException();
    }

    public override Window CreateProgressWindow()
    {
      throw new NotImplementedException();
    }
  }
}
