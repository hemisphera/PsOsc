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
using Hsp.PsOsc.Infrastructure;
using TimeSpan = System.TimeSpan;

namespace Hsp.PsOsc.Controls
{
  /// <summary>
  /// Interaction logic for StatusPartControl.xaml
  /// </summary>
  public partial class StatusPartControl : UserControl
  {
    
    private static readonly Brush GreenColorBrush = new SolidColorBrush(Colors.Green.ChangeAlpha(50));

    private static readonly Brush TransparentColorBrush = new SolidColorBrush(Colors.Transparent);


    public StatusPartControl()
    {
      InitializeComponent();
    }


    private void PlayingColorConverter_OnOnConvert(object sender, ConverterEventArgs e)
    {
      var isSet = (bool) e.Value;
      e.Result = isSet ? GreenColorBrush : TransparentColorBrush;
    }

    private static string FormatTime(TimeSpan ts)
    {
      var ms = ts.Milliseconds / 100;
      if (ts.Hours == 0)
        return $"{ts.Minutes:D2}:{ts.Seconds:D2}.{ms:D2}";
      return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}.{ms:D2}";
    }

    private void TimeSpanFormatConverter_OnOnConvert(object sender, ConverterEventArgs e)
    {
      TimeSpan ts;
      if (e.Value is TimeSpan span)
        ts = span;
      else
        ts = TimeSpan.FromSeconds((float) e.Value);

      e.Result = FormatTime(ts);
    }

  }

}
