using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class AppShell : Shell
{
  public AppShell(IAppShellViewModel model)
  {
    BindingContext = model;

    InitializeComponent();
  }
}
