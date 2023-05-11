using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class AboutPage : ContentPage
{
  public AboutPage(IAboutPageViewModel model)
  {
    BindingContext = model;
    InitializeComponent();
  }
}