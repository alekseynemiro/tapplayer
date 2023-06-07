using System.Collections.ObjectModel;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Services;

public interface ITapPlayerService
{
  Task Set(ObservableCollection<ITileViewModel> tiles);

  Task StopAllExcludingBackground(Func<ITileViewModel, bool> additionalConditions);

  Task StopAll();

  Task DisposeAll();

  Task Clear();
}
