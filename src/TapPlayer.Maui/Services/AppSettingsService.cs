using System.Globalization;

namespace TapPlayer.Maui.Services;

public class AppSettingsService : IAppSettingsService
{
  private readonly IPreferences _preferences;

  public Guid LastProjectId
  {
    get
    {
      return Guid.Parse(
        _preferences.Get(nameof(LastProjectId), Guid.Empty.ToString())
      );
    }
    set
    {
      _preferences.Set(nameof(LastProjectId), value.ToString());
    }
  }

  public int Language
  {
    get
    {
      return _preferences.Get(nameof(Language), 0);
    }
    set
    {
      _preferences.Set(nameof(Language), value);

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
