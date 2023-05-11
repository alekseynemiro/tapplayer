using System.Windows.Input;

namespace TapPlayer.Maui.ViewModels;

public interface IAppShellViewModel
{
  string Title { get; }

  bool CanUseProjectSettings { get; set; }

  IAsyncCommand CreateProjectCommand { get; }

  IAsyncCommand OpenProjectCommand { get; }

  IAsyncCommand ProjectSettingsCommand { get; }

  IAsyncCommand AboutCommand { get; }

  ICommand ExitCommand { get; }
}
