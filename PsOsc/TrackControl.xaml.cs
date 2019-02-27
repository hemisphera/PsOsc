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
using eos.Mvvm.Converters;

namespace Hsp.PsOsc
{
  /// <summary>
  /// Interaction logic for TrackControl.xaml
  /// </summary>
  public partial class TrackControl : UserControl
  {

    private static readonly SolidColorBrush RedBrush = new SolidColorBrush(Colors.Red);
    
    private static readonly SolidColorBrush YellowBrush = new SolidColorBrush(Colors.Gold);

    private static readonly SolidColorBrush TransparentBrush = new SolidColorBrush(Colors.Transparent);


    public TrackControl()
    {
      InitializeComponent();
    }


    private void MutedColorConverter_OnOnConvert(object sender, ConverterEventArgs e)
    {
      var isSet = (bool) e.Value;
      e.Result = isSet ? RedBrush : TransparentBrush;
    }

    private void SoloedColorConverter_OnOnConvert(object sender, ConverterEventArgs e)
    {
      var isSet = (bool) e.Value;
      e.Result = isSet ? YellowBrush : TransparentBrush;
    }

    private void BoolToOpacityConverter_OnOnConvert(object sender, ConverterEventArgs e)
    {
      var isSet = (bool) e.Value;
      e.Result = isSet ? 1f : 0.4f;
    }

  }

}