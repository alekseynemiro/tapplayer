using Microsoft.Maui.Platform;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.Platforms.Android.Servies;

public class KeyboardService : IKeyboardService
{
  public void HideKeyboard()
  {
    if (Platform.CurrentActivity.CurrentFocus != null)
    {
      Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
    }
  }
}
