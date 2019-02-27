using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Hsp.PsOsc.Infrastructure
{

  internal static class Extensions
  {

    public static Color ChangeAlpha(this Color col, byte newAlpha)
    {
      col.A = newAlpha;
      return col;
    }

  }

}
