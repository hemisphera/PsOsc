using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Data.Extensions;
using DevExpress.Mvvm.Native;
using eos.Mvvm.Core;
using Hsp.PsOsc.Infrastructure;

namespace Hsp.PsOsc.Parts
{

  public class SetlistPart : AsyncItemsViewModelBase<SetlistItem>, IVieWModel
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

    public UiCommand MoveUpCommand => GetAutoFieldValue(new UiCommand
    {
      Image = "MoveUp",
      Title = "Move Up",
      ExecuteFunction = async parameter => MoveUp()
    });

    public UiCommand MoveDownCommand => GetAutoFieldValue(new UiCommand
    {
      Image = "MoveDown",
      Title = "Move Down",
      ExecuteFunction = async parameter => MoveDown()
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

    public IView View { get; set; }

    private SetlistItem PlayingItem { get; set; }



    public SetlistPart() : base(new ObservableCollection<SetlistItem>())
    {
      Engine.Instance.RegionFinished += (s, e) =>
      {
        Debug.WriteLine($"Region {e.Name} finished");
        PlayNext();
      };
    }

 
    protected override async Task<IEnumerable<SetlistItem>> GetItems()
    {
      return await Task.FromResult(Items.ToArray());
    }


    public void Play(SetlistItem item = null)
    {
      Task.Run(() =>
      {
        if (item == null)
          item = SelectedItem;

        var region = item?.FindRegion();
        if (region == null) return;

        PlayingItem = item;
        if (Math.Abs(PlayingItem.PauseBefore) > 0.1)
          Thread.Sleep(TimeSpan.FromSeconds(PlayingItem.PauseBefore));

        Debug.WriteLine($"Playing region {region.Name}");
        Engine.Instance.Play(region);
      });
    }

    public void PlayNext()
    {
      Task.Run(() =>
      {
        if (PlayingItem == null) return;

        if (Math.Abs(PlayingItem.PauseAfter) > 0.1)
          Thread.Sleep(TimeSpan.FromSeconds(PlayingItem.PauseAfter));

        var nextItem = Items.FirstOrDefault(i => i.Sequence > PlayingItem.Sequence);
        Debug.WriteLine("Next: " + nextItem.Name);
        Play(nextItem);
      });
    }

    public void LoadFromFile(string filename = null)
    {
      if (String.IsNullOrEmpty(filename))
      {
        var fre = new FileRequestEventArgs
        {
          Filter = "JSON File (*.json)|*.json",
          Title = "Open Setlist",
          Type = FileRequestEventArgs.RequestType.OpenFile
        };
        if (UiSettings.Mediator.RequestFile(fre))
          filename = fre.SelectedPath;
      }

      if (String.IsNullOrEmpty(filename))
        return;

      var setlist = new Setlist();
      setlist.LoadFromFile(filename);
      Items.Clear();
      foreach (var item in setlist.Songs)
        Items.Add(item);
    }

    public void SaveToFile(string filename = null)
    {
      if (String.IsNullOrEmpty(filename))
      {
        var fre = new FileRequestEventArgs
        {
          Filter = "JSON File (*.json)|*.json",
          Title = "Save Setlist",
          Type = FileRequestEventArgs.RequestType.SaveFile
        };
        if (UiSettings.Mediator.RequestFile(fre))
          filename = fre.SelectedPath;
      }

      if (String.IsNullOrEmpty(filename))
        return;

      var setlist = new Setlist();
      setlist.Songs.AddRange(Items);
      setlist.SaveToFile(filename);
    }

    
    public void MoveUp(SetlistItem item = null)
    {
      if (item == null) item = SelectedItem;
      if (item == null) return;

      var items = Items.OrderBy(i => i.Sequence).ToList();
      var currIndex = items.IndexOf(item);
      if (currIndex <= 0) return;

      var newItem = items[currIndex - 1];
      SwapSequence(item, newItem);
      View?.RefreshView();
    }

    public void MoveDown(SetlistItem item = null)
    {
      if (item == null) item = SelectedItem;
      if (item == null) return;

      var items = Items.OrderBy(i => i.Sequence).ToList();
      var currIndex = items.IndexOf(item);
      if (currIndex < 0 || currIndex >= items.Count - 1) return;

      var newItem = items[currIndex + 1];
      SwapSequence(item, newItem);
      View?.RefreshView();
    }
    
    private void SwapSequence(SetlistItem item, SetlistItem newItem)
    {
      var seq = item.Sequence;
      item.Sequence = newItem.Sequence;
      newItem.Sequence = seq;
    }

  }

}