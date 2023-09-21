using System.Windows.Input;

namespace TapPlayer.Maui.ViewModels;

public interface IAppShellViewModel
{
  string Title { get; }

  bool CanUseProjectSettings { get; set; }

  bool ShowCloseProjectItem { get; set; }

  IAsyncCommand CreateProjectCommand { get; }

  IAsyncCommand OpenProjectCommand { get; }

  IAsyncCommand ProjectSettingsCommand { get; }

  IAsyncCommand ApplicationSettingsCommand { get; }

  IAsyncCommand AboutCommand { get; }

  ICommand ExitCommand { get; }
}
