using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class AppShellViewModel : ViewModelBase, IAppShellViewModel
{
  private readonly IAppInfo _appInfo;
  private readonly INavigationService _navigationService;
  private readonly IActiveProjectService _activeProjectService;
  private bool _canUseProjectSettings = false;

  public string Title
  {
    get
    {
      return $"TapPlayer v{_appInfo.VersionString}";
    }
  }

  public bool CanUseProjectSettings
  {
    get
    {
      return _canUseProjectSettings;
    }
    set
    {
      _canUseProjectSettings = value;
      OnProprtyChanged();
    }
  }

  public IAsyncCommand CreateProjectCommand { get; }

  public IAsyncCommand OpenProjectCommand { get; }

  public IAsyncCommand ProjectSettingsCommand { get; }

  public IAsyncCommand AboutCommand { get; }

  public ICommand ExitCommand { get; }

  public AppShellViewModel(
    IAppInfo appInfo,
    INavigationService navigationService,
    IActiveProjectService activeProjectService
  )
  {
    _appInfo = appInfo;
    _navigationService = navigationService;
    _activeProjectService = activeProjectService;

    CreateProjectCommand = new AsyncCommand(_navigationService.CreateProject);

    OpenProjectCommand = new AsyncCommand(_navigationService.ProjectList);

    ProjectSettingsCommand = new AsyncCommand(
      () =>
      {
        // TODO: remove workaround if disabling menu item works
        if (CanUseProjectSettings)
        {
          return _navigationService.ProjectSettings(_activeProjectService.ProjectId);
        }
        // workaroud, because the menu item disabling does not work
        else
        {
          return _navigationService.ProjectList();
        }
      }
      // () => CanUseProjectSettings
    );

    AboutCommand = new AsyncCommand(_navigationService.About);

    ExitCommand = new Command(Application.Current.Quit);

    WeakReferenceMessenger.Default.Register<IActiveProjectService>(
      this,
      (r, m) =>
      {
        CanUseProjectSettings = m.HasProject;
      }
    );
  }
}
