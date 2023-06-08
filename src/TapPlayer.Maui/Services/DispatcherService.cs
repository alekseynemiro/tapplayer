namespace TapPlayer.Maui.Services;

public class DispatcherService : IDispatcherService
{
  public void Dispatch(Action action)
  {
    Application.Current.MainPage.Dispatcher.Dispatch(action);
  }

  public Task DispatchAsync(Action action)
  {
    return Application.Current.MainPage.Dispatcher.DispatchAsync(action);
  }
}
