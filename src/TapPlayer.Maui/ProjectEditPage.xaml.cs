using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class ProjectEditPage : ContentPage
{
  public ProjectEditPage(
    IActiveProjectService activeProjectService,
    IProjectEditViewModel model
  )
  {
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
}
