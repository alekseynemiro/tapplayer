using System.Collections.ObjectModel;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Services;

public interface ITapPlayerService
{
  void Set(ObservableCollection<ITileViewModel> tiles);

  void StopAllExcludingBackground();

  void StopAll();

  void DisposeAll();

  void Clear();
}
