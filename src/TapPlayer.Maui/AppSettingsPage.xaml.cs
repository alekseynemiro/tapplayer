using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class AppSettingsPage : ContentPage
{
  public AppSettingsPage(IAppSettingsPageViewModel model)
  {
    BindingContext = model;
    InitializeComponent();
  }
}
