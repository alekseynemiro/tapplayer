using System.Globalization;

namespace TapPlayer.Maui.Services;

public class AppSettingsService : IAppSettingsService
{
  private readonly IPreferences _preferences;

  public int Language
  {
    get
    {
      return _preferences.Get("Language", 0);
    }
    set
    {
      _preferences.Set("Language", value);

      if (value == 0)
      {
        Thread.CurrentThread.CurrentCulture =
          Thread.CurrentThread.CurrentUICulture =
            CultureInfo.CurrentUICulture;
      }
      else
      {
        Thread.CurrentThread.CurrentCulture =
          Thread.CurrentThread.CurrentUICulture =
            CultureInfo.GetCultureInfo(value);
      }
    }
  }

  public AppSettingsService(IPreferences preferences)
  {
    _preferences = preferences;
  }
}
