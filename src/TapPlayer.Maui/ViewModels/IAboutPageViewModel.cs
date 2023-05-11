using System.Windows.Input;

namespace TapPlayer.Maui.ViewModels;

public interface IAboutPageViewModel
{
  string Name { get; }
  string Version { get; }
  ICommand OkCommand { get; }
}
