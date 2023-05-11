namespace TapPlayer.Maui;

public partial class App : Application
{
  public App(IServiceProvider serviceProvider)
  {
    InitializeComponent();

    Current.UserAppTheme = AppTheme.Light;
    MainPage = serviceProvider.GetRequiredService<AppShell>();
  }
}
