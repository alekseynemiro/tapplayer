using Android.App;
using Android.Runtime;
using TapPlayer.Maui.Services;
using AndroidKeyboardService = TapPlayer.Maui.Platforms.Android.Servies.KeyboardService;

namespace TapPlayer.Maui;

[Application]
public class MainApplication : MauiApplication
{
  public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
  {
  }

  protected override MauiApp CreateMauiApp()
  {
    var builder = AppBuilder.CreateBuilder();

    builder.Services.AddSingleton<IKeyboardService, AndroidKeyboardService>();

    return builder.Build();
  }
}
