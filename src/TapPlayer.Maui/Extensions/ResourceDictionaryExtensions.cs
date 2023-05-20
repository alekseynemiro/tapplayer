namespace TapPlayer.Maui.Extensions;

internal static class ResourceDictionaryExtensions
{
  public static object FindResource(this ResourceDictionary resourceDictionary, string key)
  {
    foreach (var resource in resourceDictionary.MergedDictionaries)
    {
      if (resource.Keys.Contains(key))
      {
        return resource[key];
      }
    }

    return null;
  }
}
