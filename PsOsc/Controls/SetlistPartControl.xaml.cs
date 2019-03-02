using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hsp.PsOsc.Infrastructure;
using Hsp.PsOsc.Parts;

namespace Hsp.PsOsc.Controls
{
  /// <summary>
  /// Interaction logic for SetlistPartControl.xaml
  /// </summary>
  public partial class SetlistPartControl : UserControl, IView
  {
    
    public SetlistPartControl()
    {
      InitializeComponent();
      DataContextChanged += MainVm.OnDataContextChanged;
    }


    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var vm = DataContext as SetlistPart;
      vm?.Play();
    }

    public void RefreshView()
    {
      zheGrid.RefreshData();
    }

  }

}