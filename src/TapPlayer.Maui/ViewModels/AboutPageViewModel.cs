using System.Windows.Input;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class AboutPageViewModel : IAboutPageViewModel
{
  private readonly IAppInfo _appInfo;

  public string Name => _appInfo.Name;

  public string Version => $"v{_appInfo.VersionString}";

  public ICommand OkCommand { get; init; }

  public AboutPageViewModel(
    IAppInfo appInfo,
    INavigationService navigationService
  )
  {
    _appInfo = appInfo;
    OkCommand = new Command(() => navigationService.PopAsync());
  }
}
