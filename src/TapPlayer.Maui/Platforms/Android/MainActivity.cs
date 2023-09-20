using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;

namespace TapPlayer.Maui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
  protected override void OnCreate(Bundle savedInstanceState)
  {
    base.OnCreate(savedInstanceState);

    if (OperatingSystem.IsAndroidVersionAtLeast(31))
    {
      var uiModeManager = (UiModeManager)GetSystemService(UiModeService);
      uiModeManager.SetApplicationNightMode(1);
    }
    else
    {
      Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
    }
  }
}
