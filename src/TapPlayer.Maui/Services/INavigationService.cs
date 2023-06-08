namespace TapPlayer.Maui.Services;

public interface INavigationService
{
  bool IsNavigating { get; }

  Task Home();

  Task ProjectList();

  Task CreateProject();

  Task OpenProject(Guid projectId);

  Task ProjectSettings(Guid projectId);

  Task CloseProjectEditor();

  Task ApplicationSettings();

  Task About();

  Task PushAsync(Page page);

  Task PushModalAsync(Page page);

  Task PopAsync();

  Task PopModalAsync();
}
