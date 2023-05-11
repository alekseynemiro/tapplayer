namespace TapPlayer.Maui;

public static class MauiProgram
{
  public static IServiceProvider ServiceProvider =>
#if WINDOWS
  MauiWinUIApplication.Current.Services;
#elif ANDROID
  MauiApplication.Current.Services;

#else
  null;

#endif

  public static MauiApp CreateMauiApp()
  {
    var builder = AppBuilder.CreateBuilder();

    return builder.Build();
  }
}
