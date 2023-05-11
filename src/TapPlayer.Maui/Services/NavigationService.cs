using TapPlayer.Core.Services.Projects;

namespace TapPlayer.Maui.Services;

public class NavigationService : INavigationService
{
  private readonly IServiceProvider _serviceProvider;
  private readonly IActiveProjectService _activeProjectService;
  private readonly IKeyboardService _keyboardService;
  private readonly IProjectListService _projectListService;
  private readonly ITapPlayerService _tapPlayerService;

  private INavigation Navigation
  {
    get => AppShell.Navigation;
  }

  private AppShell AppShell
  {
    get => (AppShell)Application.Current.MainPage;
  }

  public bool IsNavigating { get; private set; }

  public NavigationService(
    IServiceProvider serviceProvider,
    IActiveProjectService activeProjectService,
    IKeyboardService keyboardService,
    IProjectListService projectListService,
    ITapPlayerService tapPlayerService
  )
  {
    _serviceProvider = serviceProvider;
    _activeProjectService = activeProjectService;
    _keyboardService = keyboardService;
    _projectListService = projectListService;
    _tapPlayerService = tapPlayerService;
  }

  public async Task CreateProject()
  {
    IsNavigating = true;

    _activeProjectService.Reset();

    var projectEditPage = _serviceProvider.GetRequiredService<ProjectEditPage>();

    await PushAsync(projectEditPage);

    IsNavigating = false;
  }

  public Task OpenProject(Guid projectId)
  {
    IsNavigating = true;

    _activeProjectService.Set(projectId);

    Application.Current.MainPage = _serviceProvider.GetRequiredService<AppShell>();

    IsNavigating = false;

    return Task.CompletedTask;
  }

  public async Task ProjectSettings(Guid projectId)
  {
    IsNavigating = true;

    _activeProjectService.Set(projectId);

    var projectSettingsPage = _serviceProvider.GetRequiredService<ProjectSettingsPage>();

    await PushAsync(projectSettingsPage);

    IsNavigating = false;
  }

  public async Task CloseProjectEditor()
  {
    IsNavigating = true;

    var isProjectSettingsPage = Navigation.NavigationStack.LastOrDefault() is ProjectSettingsPage;
    var prevPage = Navigation.NavigationStack.TakeLast(2).FirstOrDefault();

    if (prevPage is ProjectListPage)
    {
      await PopAsync();
    }
    else
    {
      if (!isProjectSettingsPage)
      {
        var projectList = await _projectListService.GetAll();

        if (projectList.Items.Count == 1)
        {
          _activeProjectService.Set(projectList.Items.Single().Id);
        }
        else
        {
          _activeProjectService.Reset();
        }
      }

      await Home();
    }

    IsNavigating = false;
  }

  public async Task ProjectList()
  {
    IsNavigating = true;

    var projectListPage = _serviceProvider.GetRequiredService<ProjectListPage>();

    await PushAsync(projectListPage);

    IsNavigating = false;
  }

  public Task Home()
  {
    IsNavigating = true;

    StopAllPlayers();

    Application.Current.MainPage = _serviceProvider.GetRequiredService<AppShell>();

    IsNavigating = false;

    return Task.CompletedTask;
  }

  public async Task About()
  {
    IsNavigating = true;

    var aboutPage = _serviceProvider.GetRequiredService<AboutPage>();

    await PushAsync(aboutPage);

    IsNavigating = false;
  }

  public Task PushAsync(Page page)
  {
    if (IsPageOpen(page.GetType()))
    {
      return Task.CompletedTask;
    }

    StopAllPlayers();
    HideUnnecessaryUIElements();

    return Navigation.PushAsync(page);
  }

  public Task PushModalAsync(Page page)
  {
    if (IsPageOpen(page.GetType()))
    {
      return Task.CompletedTask;
    }

    StopAllPlayers();
    HideUnnecessaryUIElements();

    return Navigation.PushModalAsync(page);
  }

  public Task PopAsync()
  {
    StopAllPlayers();
    HideUnnecessaryUIElements();
    return Navigation.PopAsync();
  }

  public Task PopModalAsync()
  {
    StopAllPlayers();
    HideUnnecessaryUIElements();
    return Navigation.PopModalAsync();
  }

  private bool IsPageOpen(Type type)
  {
    return Navigation.NavigationStack.LastOrDefault()?.GetType() == type
      || Navigation.ModalStack.LastOrDefault()?.GetType() == type;
  }

  private async Task Clear()
  {
    while (Navigation.ModalStack.Count > 0)
    {
      await Navigation.PopModalAsync();
    }

    while (Navigation.NavigationStack.Count > 1)
    {
      await Navigation.PopAsync();
    }
  }

  private void HideUnnecessaryUIElements()
  {
    Shell.Current.FlyoutIsPresented = false;
    _keyboardService.HideKeyboard();
  }

  private void StopAllPlayers()
  {
    _tapPlayerService.StopAll();
  }
}
