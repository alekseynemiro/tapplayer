using System.Collections.ObjectModel;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Services;

public class TapPlayerService : ITapPlayerService
{
  private readonly IDispatcherService _dispatcherService;
  private readonly ObservableCollection<ITileViewModel> _tiles = new ObservableCollection<ITileViewModel>();

  public TapPlayerService(IDispatcherService dispatcherService)
  {
    _dispatcherService = dispatcherService;
  }

  public void Set(ObservableCollection<ITileViewModel> tiles)
  {
    Clear();

    foreach (var tile in tiles)
    {
      _tiles.Add(tile);
    }
  }

  public void StopAllExcludingBackground(Func<ITileViewModel, bool> additionalConditions)
  {
    var activeTiles = _tiles
      .Where(x => x.Player != null && x.Player.IsPlaying)
      .Where(x => !x.IsBackground)
      .Where(additionalConditions)
      .ToList();

    foreach (var tile in activeTiles)
    {
      _dispatcherService.Dispatch(tile.Player.Stop);
    }
  }

  public void StopAll()
  {
    var activeTiles = _tiles
      .Where(x => x.Player != null && x.Player.IsPlaying)
      .ToList();

    foreach (var tile in activeTiles)
    {
      _dispatcherService.Dispatch(tile.Player.Stop);
    }
  }

  public void DisposeAll()
  {
    foreach (var tile in _tiles)
    {
      _dispatcherService.Dispatch(() =>
      {
        if (tile.Player != null)
        {
          tile.Player.Stop();
          tile.Player.Dispose();
          tile.Player = null;
        }
      });
    }
  }

  public void Clear()
  {
    DisposeAll();
    _tiles.Clear();
  }
}
