using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class ProjectSettingsPage : ContentPage
{
  public ProjectSettingsPage(
    IActiveProjectService activeProjectService,
    IProjectEditViewModel model
  )
  {
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
}
