using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using eos.Mvvm.Core;

namespace Hsp.PsOsc.Parts
{

  public class RegionsPart : AsyncItemsViewModelBase<RegionSlot>
  {

    public RegionsPart() : base(Engine.Instance.RegionsInternal)
    {
    }

    protected override async Task<IEnumerable<RegionSlot>> GetItems()
    {
      return await Task.FromResult(Items.ToArray());
    }

  }

}