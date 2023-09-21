using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class AppShellViewModel : ViewModelBase, IAppShellViewModel
{
  private readonly IAppInfo _appInfo;
  private readonly INavigationService _navigationService;
  private readonly IActiveProjectService _activeProjectService;
  private bool _showProjectSettingsItem = false;
  private bool _showCloseProjectItem = false;

  public string Title
  {
    get
    {
      return $"TapPlayer v{_appInfo.VersionString}";
    }
  }

  public bool ShowProjectSettingsItem
  {
    get
    {
      return _showProjectSettingsItem;
    }
    set
    {
      _showProjectSettingsItem = value;
      OnProprtyChanged();
    }
  }

  public bool ShowCloseProjectItem
  {
    get
    {
      return _showCloseProjectItem;
    }
    set
    {
      _showCloseProjectItem = value;
      OnProprtyChanged();
    }
  }

  public IAsyncCommand CreateProjectCommand { get; }

  public IAsyncCommand OpenProjectCommand { get; }

  public ICommand CloseProjectCommand { get; }

  public IAsyncCommand ProjectSettingsCommand { get; }

  public IAsyncCommand ApplicationSettingsCommand { get; }

  public IAsyncCommand AboutCommand { get; }

  public ICommand ExitCommand { get; }

  public AppShellViewModel(
    IAppInfo appInfo,
    INavigationService navigationService,
    IActiveProjectService activeProjectService,
    IAppSettingsService appSettingsService
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
        return _navigationService.ProjectSettings(_activeProjectService.ProjectId);
      }
    );

    ApplicationSettingsCommand = new AsyncCommand(_navigationService.ApplicationSettings);

    AboutCommand = new AsyncCommand(_navigationService.About);

    ExitCommand = new Command(Application.Current.Quit);

    CloseProjectCommand = new Command(
      () =>
      {
        activeProjectService.Reset();
        appSettingsService.LastProjectId = Guid.Empty;
        ShowCloseProjectItem = false;
        Shell.Current.FlyoutIsPresented = false;
      }
    );

    WeakReferenceMessenger.Default.Register<IActiveProjectService>(
      this,
      (r, m) =>
      {
        ShowProjectSettingsItem = m.HasProject;
        ShowCloseProjectItem = m.HasProject;
      }
    );
  }
}
