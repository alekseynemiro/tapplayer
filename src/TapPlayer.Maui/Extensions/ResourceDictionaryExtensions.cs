namespace TapPlayer.Maui.Extensions;

internal static class ResourceDictionaryExtensions
{
  public static object FindResource(this ResourceDictionary @this, string key)
  {
    foreach (var resource in @this.MergedDictionaries)
    {
      if (resource.Keys.Contains(key))
      {
        return resource[key];
      }
    }

    return null;
  }
}
