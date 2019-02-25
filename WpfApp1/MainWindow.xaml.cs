﻿using System;
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

namespace WpfApp1
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      DataContext = MainVm.Instance;
    }

    private static string FormatTime(float? ptime)
    {
      var time = ptime ?? 0;
      if (Math.Abs(time) < 0.001)
        return "";
      var ts = TimeSpan.FromSeconds(time);
      var ms = ts.Milliseconds / 100;
      if (ts.Hours == 0)
        return $"{ts.Minutes:D2}:{ts.Seconds:D2}.{ms:D2}";
      return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}.{ms:D2}";
    }

    private void TimeSpanFormatConverter_OnOnConvert(object sender, ConverterEventArgs e)
    {
      e.Result = FormatTime((float?) e.Value);
    }

  }

}