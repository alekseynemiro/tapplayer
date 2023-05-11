using System.Collections.ObjectModel;

namespace TapPlayer.Maui.Extensions;

internal static class ObservableCollectionExtensions
{
  public static void UpdateItem<T>(this ObservableCollection<T> collection, T item)
  {
    collection[collection.IndexOf(item)] = item;
  }
}
