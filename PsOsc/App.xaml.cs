using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using DevExpress.Images;
using eos.Mvvm.Converters;

namespace Hsp.PsOsc
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {

    private void GlyphConverter_OnOnConvert(object sender, ConverterEventArgs e)
    {
      var glyphName = e.Value as string;
      if (String.IsNullOrEmpty(glyphName)) return;

      glyphName = $"{glyphName}_16x16.png";

      e.Result = null;

      var img = ImagesAssemblyImageList.Images.FirstOrDefault(i => i.Name.Equals(glyphName));
      if (img == null) return;

      e.Result = new BitmapImage(new Uri(img.MakeUri()));
    }
    
    private void InverseBoolConverter_OnOnConvert(object sender, ConverterEventArgs e)
    {
      var isSet = (bool) e.Value;
      e.Result = !isSet;
    }

  }

}