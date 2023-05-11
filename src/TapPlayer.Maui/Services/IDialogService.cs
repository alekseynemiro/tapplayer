namespace TapPlayer.Maui.Services;

public interface IDialogService
{
  Task Alert(string message, string title = null);

  Task<bool> Confirm(string message, string title = null, string accept = null, string cancel = null);

  Task Error(string message);
}
