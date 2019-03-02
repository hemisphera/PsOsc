using System;
using System.Linq;
using eos.Mvvm.Core;

namespace Hsp.PsOsc
{

  public class SetlistItem : ViewModelBase
  {

    public string Name
    {
      get => GetAutoFieldValue<string>();
      set => SetAutoFieldValue(value);
    }

    public string RegionName
    {
      get => GetAutoFieldValue<string>();
      set => SetAutoFieldValue(value);
    }


    public RegionSlot FindRegion()
    {
      if (String.IsNullOrEmpty(RegionName)) return null;
      return Engine.Instance
        .RegionsInternal
        .FirstOrDefault(r => r.Name?.Equals(RegionName) ?? false);
    }


  }

}