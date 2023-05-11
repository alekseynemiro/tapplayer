namespace TapPlayer.Maui.Extensions;

internal static class ViewExtensions
{
  public static void DispatchInvalidateMeasure(this IView view)
  {
    Application.Current.MainPage.Dispatcher.Dispatch(() => view.InvalidateMeasure());
  }
}
