using System;

namespace Hsp.PsOsc.Extensibility
{

  public class ItemEventArgs<T> : EventArgs
  {

    public T Item { get; }


    public ItemEventArgs(T item)
    {
      Item = item;
    }

  }

}