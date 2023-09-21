using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace TapPlayer.Maui.Services;

public class ToastNotificationService : IToastNotificationService
{
  public async Task Error(string message)
  {
    var snackbarOptions = new SnackbarOptions
    {
      BackgroundColor = Colors.Red,
      TextColor = Colors.White,
      ActionButtonTextColor = Colors.White,
      CornerRadius = new CornerRadius(10),
      Font = Font.SystemFontOfSize(14),
      ActionButtonFont = Font.SystemFontOfSize(14),
      CharacterSpacing = 0.5
    };

    var snackbar = Snackbar.Make(
      message,
      duration: TimeSpan.FromSeconds(3),
      visualOptions: snackbarOptions
    );

    await snackbar.Show();
  }
}
