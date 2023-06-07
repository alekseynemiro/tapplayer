using Microsoft.Extensions.Logging;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class ProjectEditPage : ContentPage
{
  private readonly ILogger _logger;

  public ProjectEditPage(
    ILogger<ProjectEditPage> logger,
    IActiveProjectService activeProjectService,
    IProjectEditViewModel model
  )
  {
    _logger = logger;

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
  }
}
