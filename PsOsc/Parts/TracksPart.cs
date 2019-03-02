using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eos.Mvvm.Core;

namespace Hsp.PsOsc.Parts
{

  public class TracksPart : AsyncItemsViewModelBase<TrackSlot>
  {

    public TracksPart() : base(Engine.Instance.TracksInternal)
    {
    }


    protected override async Task<IEnumerable<TrackSlot>> GetItems()
    {
      return await Task.FromResult(Items.ToArray());
    }

  }

}