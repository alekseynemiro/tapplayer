using TapPlayer.Maui.Resources.Strings;

namespace TapPlayer.Maui.Services;

internal class DialogService : IDialogService
{
  public Task Alert(string message, string title = null)
  {
    return Application.Current.MainPage.DisplayAlert(
      title ?? "TapPalyer",
      message,
      CommonStrings.Ok
    );
  }

  public Task<bool> Confirm(string message, string title = null, string accept = null, string cancel = null)
  {
    return Application.Current.MainPage.DisplayAlert(
      title ?? CommonStrings.Confirmation,
      message,
      accept ?? CommonStrings.Ok,
      cancel ?? CommonStrings.Cancel
    );
  }

  public Task Error(string message)
  {
    return Application.Current.MainPage.DisplayAlert(
      CommonStrings.Error,
      message,
      CommonStrings.Cancel
    );
  }
}
