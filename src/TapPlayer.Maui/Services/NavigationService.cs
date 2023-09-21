using Microsoft.Extensions.Logging;
using TapPlayer.Core.Services.Projects;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Services;

public class NavigationService : INavigationService
{
  private readonly ILogger _logger;
  private readonly IServiceProvider _serviceProvider;
  private readonly IActiveProjectService _activeProjectService;
  private readonly IKeyboardService _keyboardService;
  private readonly IProjectListService _projectListService;
  private readonly ITapPlayerService _tapPlayerService;
  private readonly IAppSettingsService _appSettingsService;

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
    ILogger<NavigationService> logger,
    IServiceProvider serviceProvider,
    IActiveProjectService activeProjectService,
    IKeyboardService keyboardService,
    IProjectListService projectListService,
    ITapPlayerService tapPlayerService,
    IAppSettingsService appSettingsService
  )
  {
    _logger = logger;
    _serviceProvider = serviceProvider;
    _activeProjectService = activeProjectService;
    _keyboardService = keyboardService;
    _projectListService = projectListService;
    _tapPlayerService = tapPlayerService;
    _appSettingsService = appSettingsService;
  }

  public async Task CreateProject()
  {
    _logger.LogDebug(nameof(CreateProject));

    IsNavigating = true;

    _activeProjectService.Reset();

    var projectEditPage = _serviceProvider.GetRequiredService<ProjectEditPage>();

    await PushAsync(projectEditPage);

    IsNavigating = false;
  }

  public Task OpenProject(Guid projectId)
  {
    _logger.LogDebug(nameof(OpenProject) + " {ProjectId}", projectId);

    IsNavigating = true;

    _activeProjectService.Set(projectId);
    _appSettingsService.LastProjectId = projectId;

    RecreateMainPage();

    IsNavigating = false;

    return Task.CompletedTask;
  }

  public async Task EditProject(Guid projectId)
  {
    _logger.LogDebug(nameof(EditProject) + " {ProjectId}", projectId);

    IsNavigating = true;

    _activeProjectService.Set(projectId);

    var projectEditPage = _serviceProvider.GetRequiredService<ProjectEditPage>();

    await PushAsync(projectEditPage);

    IsNavigating = false;
  }

  public async Task ProjectSettings(Guid projectId)
  {
    _logger.LogDebug(nameof(ProjectSettings) + " {ProjectId}", projectId);

    IsNavigating = true;

    _activeProjectService.Set(projectId);

    var projectSettingsPage = _serviceProvider.GetRequiredService<ProjectSettingsPage>();

    await PushAsync(projectSettingsPage);

    IsNavigating = false;
  }

  public async Task CloseProjectEditor()
  {
    _logger.LogDebug(nameof(CloseProjectEditor));

    IsNavigating = true;

    var isProjectSettingsPage = Navigation.NavigationStack.LastOrDefault() is ProjectSettingsPage;
    var prevPage = Navigation.NavigationStack.TakeLast(2).FirstOrDefault();

    if (prevPage is ProjectListPage)
    {
      ((IProjectListPageViewModel)prevPage.BindingContext).LoadCommand.Execute(null);
      await PopAsync();
    }
    else
    {
      if (isProjectSettingsPage)
      {
        await Home();
      }
      else
      {
        var projectList = await _projectListService.GetAll();

        if (projectList.Items.Count == 1)
        {
          _activeProjectService.Set(projectList.Items.Single().Id);
          _appSettingsService.LastProjectId = _activeProjectService.ProjectId;
        }
        else
        {
          _activeProjectService.Reset();
        }

        await Home();
      }
    }

    IsNavigating = false;
  }

  public async Task ApplicationSettings()
  {
    _logger.LogDebug(nameof(ApplicationSettings));

    IsNavigating = true;

    var appSettingsPage = _serviceProvider.GetRequiredService<AppSettingsPage>();

    await PushAsync(appSettingsPage);

    IsNavigating = false;
  }

  public async Task ProjectList()
  {
    _logger.LogDebug(nameof(ProjectList));

    IsNavigating = true;

    var projectListPage = _serviceProvider.GetRequiredService<ProjectListPage>();

    await PushAsync(projectListPage);

    IsNavigating = false;
  }

  public async Task Home()
  {
    _logger.LogDebug(nameof(Home));

    IsNavigating = true;

    await StopAllPlayers();

    RecreateMainPage();

    IsNavigating = false;
  }

  public async Task About()
  {
    _logger.LogDebug(nameof(About));

    IsNavigating = true;

    var aboutPage = _serviceProvider.GetRequiredService<AboutPage>();

    await PushAsync(aboutPage);

    IsNavigating = false;
  }

  public async Task PushAsync(Page page)
  {
    _logger.LogDebug(nameof(PushAsync));

    if (IsPageOpen(page.GetType()))
    {
      return;
    }

    await StopAllPlayers();
    await HideUnnecessaryUIElements();
    await Navigation.PushAsync(page);
  }

  public async Task PushModalAsync(Page page)
  {
    _logger.LogDebug(nameof(PushModalAsync));

    if (IsPageOpen(page.GetType()))
    {
      return;
    }

    await StopAllPlayers();
    await HideUnnecessaryUIElements();
    await Navigation.PushModalAsync(page);
  }

  public async Task PopAsync()
  {
    _logger.LogDebug(nameof(PopAsync));

    await StopAllPlayers();
    await HideUnnecessaryUIElements();
    await Navigation.PopAsync();
  }

  public async Task PopModalAsync()
  {
    _logger.LogDebug(nameof(PopModalAsync));

    await StopAllPlayers();
    await HideUnnecessaryUIElements();
    await Navigation.PopModalAsync();
  }

  private bool IsPageOpen(Type type)
  {
    var result = Navigation.NavigationStack.LastOrDefault()?.GetType() == type
      || Navigation.ModalStack.LastOrDefault()?.GetType() == type;

    _logger.LogDebug(nameof(IsPageOpen) + " {Type} = {Result}", type, result);

    return result;
  }

  private async Task Clear()
  {
    _logger.LogDebug(nameof(Clear));

    while (Navigation.ModalStack.Count > 0)
    {
      await Navigation.PopModalAsync(false);
    }

    while (Navigation.NavigationStack.Count > 1)
    {
      await Navigation.PopAsync(false);
    }
  }

  private Task HideUnnecessaryUIElements()
  {
    _logger.LogDebug(nameof(HideUnnecessaryUIElements));

    Shell.Current.FlyoutIsPresented = false;
    _keyboardService.HideKeyboard();

    return Task.CompletedTask;
  }

  private Task StopAllPlayers()
  {
    _logger.LogDebug(nameof(StopAllPlayers));

    return _tapPlayerService.StopAll();
  }

  private void RecreateMainPage()
  {
    _logger.LogDebug(nameof(RecreateMainPage));

    Application.Current.MainPage = _serviceProvider.GetRequiredService<AppShell>();
  }
}
