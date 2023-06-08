using System.Globalization;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui;

public partial class App : Application
{
  public App(IServiceProvider serviceProvider)
  {
    InitializeComponent();

    var appSettings = serviceProvider.GetRequiredService<IAppSettingsService>();
    var activeProjectService = serviceProvider.GetRequiredService<IActiveProjectService>();

    if (appSettings.Language != 0)
    {
      Thread.CurrentThread.CurrentCulture =
        Thread.CurrentThread.CurrentUICulture =
          CultureInfo.GetCultureInfo(appSettings.Language);
    }

    activeProjectService.Set(appSettings.LastProjectId);

    Current.UserAppTheme = AppTheme.Light;
    MainPage = serviceProvider.GetRequiredService<AppShell>();
  }
}
