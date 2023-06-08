using Microsoft.Extensions.Logging;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class ProjectSettingsPage : ContentPage
{
  private readonly ILogger _logger;

  public ProjectSettingsPage(
    ILogger<ProjectSettingsPage> logger,
    IActiveProjectService activeProjectService,
    IProjectEditViewModel model
  )
  {
    _logger = logger;

    _logger.LogDebug("Instance created.");

    if (!activeProjectService.HasProject)
    {
      throw new ArgumentNullException(nameof(activeProjectService.ProjectId));
    }

    BindingContext = model;

    InitializeComponent();

    Dispatcher.Dispatch(async () =>
    {
      await model.LoadCommand.ExecuteAsync(activeProjectService.ProjectId);
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
  }
}
