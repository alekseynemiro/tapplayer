using MauiAppInfo = Microsoft.Maui.ApplicationModel.AppInfo;

namespace TapPlayer.Maui.Services;

// At the current moment, MAUI has some problems with providing version number, had to override

internal class AppInfo : IAppInfo
{
  public string PackageName => MauiAppInfo.PackageName;

  public string Name => MauiAppInfo.Name;

  public string VersionString => $"{Version.Major}.{Version.Minor} (build {Version.Build})";

  public Version Version => new Version(1, 4, 20230608);

  public string BuildString => "0";

  public AppTheme RequestedTheme => AppTheme.Light;

  public AppPackagingModel PackagingModel => AppPackagingModel.Packaged;

  public LayoutDirection RequestedLayoutDirection => LayoutDirection.Unknown;

  public void ShowSettingsUI()
  {
    MauiAppInfo.ShowSettingsUI();
  }
}
