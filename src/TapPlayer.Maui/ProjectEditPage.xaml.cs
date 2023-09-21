using Microsoft.Extensions.Logging;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

// TODO: Rename to ProjectCreationPage
public partial class ProjectEditPage : ContentPage
{
  private readonly ILogger _logger;

  private readonly IActiveProjectService _activeProjectService;

  private readonly IAppSettingsService _appSettingsService;

  public ProjectEditPage(
    ILogger<ProjectEditPage> logger,
    IActiveProjectService activeProjectService,
    IAppSettingsService appSettingsService,
    IProjectEditViewModel model
  )
  {
    _logger = logger;
    _activeProjectService = activeProjectService;
    _appSettingsService = appSettingsService;

    _logger.LogDebug("Instance created.");

    BindingContext = model;

    InitializeComponent();

    Dispatcher.Dispatch(async () =>
    {
      if (activeProjectService.HasProject)
      {
        await model.LoadCommand.ExecuteAsync(activeProjectService.ProjectId);
      }
    });
  }

  protected override void OnAppearing()
  {
    _logger.LogDebug(nameof(OnAppearing));
    base.OnAppearing();
    ProjectEdit.Init();
  }

  protected override void OnDisappearing()
  {
    _logger.LogDebug(nameof(OnDisappearing));
    base.OnDisappearing();
    ProjectEdit.Dispose();

    _activeProjectService.Set(_appSettingsService.LastProjectId);
  }
}
