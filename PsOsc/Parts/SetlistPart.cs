using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eos.Mvvm.Core;

namespace Hsp.PsOsc.Parts
{

  public class SetlistPart : AsyncItemsViewModelBase<SetlistItem>
  {

    public UiCommand LockCommand => GetAutoFieldValue(new UiCommand
    {
      Image = "Cancel",
      Title = "Lock",
      ExecuteFunction = async parameter => Locked = true
    });

    public UiCommand UnlockCommand => GetAutoFieldValue(new UiCommand
    {
      Image = "Apply",
      Title = "Unlock",
      ExecuteFunction = async parameter => Locked = false
    });

    public UiCommand AddItemCommand => GetAutoFieldValue(new UiCommand
    {
      Title = "Add Item",
      Image = "AddItem",
      ExecuteFunction = async parameter => {
        var newItem = new SetlistItem();
        Items.Add(newItem);
        SelectedItem = newItem;
      },
      CanExecuteCallback = parameter => !Locked
    });

    public UiCommand RemoveItemCommand => GetAutoFieldValue(new UiCommand
    {
      Title = "Remove Item",
      Image = "RemoveItem",
      ExecuteFunction = async parameter => {
        if (SelectedItem == null) return;
        Items.Remove(SelectedItem);
      },
      CanExecuteCallback = parameter => !Locked
    });

    public UiCommand LoadCommand => GetAutoFieldValue(new UiCommand
    {
      Image = "Open",
      Title = "Load",
      ExecuteFunction = async parameter => LoadFromFile()
    });

    public UiCommand SaveCommand => GetAutoFieldValue(new UiCommand
    {
      Image = "Save",
      Title = "Save",
      ExecuteFunction = async parameter => SaveToFile()
    });


    public bool Locked
    {
      get => GetAutoFieldValue<bool>();
      set
      {
        SetAutoFieldValue(value);
        AddItemCommand.UpdateEnabled();
        RemoveItemCommand.UpdateEnabled();
      }
    }


    public SetlistPart() : base(new ObservableCollection<SetlistItem>())
    {
    }

 
    protected override async Task<IEnumerable<SetlistItem>> GetItems()
    {
      return await Task.FromResult(Items.ToArray());
    }


    public void Play(SetlistItem item = null)
    {
      if (item == null)
        item = SelectedItem;
      if (item == null) return;

      var region = item.FindRegion();
      Engine.Instance.Play(region);
    }
        

    public void LoadFromFile()
    {
      throw new NotImplementedException();
    }

    public void SaveToFile()
    {
      throw new NotImplementedException();
    }

  }

}