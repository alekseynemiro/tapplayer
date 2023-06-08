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

  public async Task Set(ObservableCollection<ITileViewModel> tiles)
  {
    await Clear();

    foreach (var tile in tiles)
    {
      _tiles.Add(tile);
    }
  }

  public async Task StopAllExcludingBackground(Func<ITileViewModel, bool> additionalConditions)
  {
    var activeTiles = _tiles
      .Where(x => x.Player != null && x.Player.IsPlaying)
      .Where(x => !x.IsBackground)
      .Where(additionalConditions)
      .ToList();

    foreach (var tile in activeTiles)
    {
      await _dispatcherService.DispatchAsync(tile.Player.Stop);
    }
  }

  public async Task StopAll()
  {
    var activeTiles = _tiles
      .Where(x => x.Player != null && x.Player.IsPlaying)
      .ToList();

    foreach (var tile in activeTiles)
    {
      await _dispatcherService.DispatchAsync(tile.Player.Stop);
    }
  }

  public async Task DisposeAll()
  {
    foreach (var tile in _tiles)
    {
      await _dispatcherService.DispatchAsync(() =>
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

  public async Task Clear()
  {
    await DisposeAll();
    _tiles.Clear();
  }
}
