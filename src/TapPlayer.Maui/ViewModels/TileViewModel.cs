using TapPlayer.Data.Enums;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class TileViewModel : ViewModelBase, ITileViewModel
{
  private readonly ITapPlayerService _tapPlayerService;

  private int _index;
  private string _name;
  private FileViewModel _file;
  private bool _isBackground;
  private ColorPalette _color;
  private IMediaPlayerViewModel _player;
  private bool _isPlayable = true;
  private bool _isLooped = false;

  public int Index
  {
    get
    {
      return _index;
    }
    set
    {
      _index = value;
      OnProprtyChanged();
    }
  }

  public string Name
  {
    get
    {
      return _name;
    }
    set
    {
      _name = value;
      OnProprtyChanged();
    }
  }

  public FileViewModel File
  {
    get
    {
      return _file;
    }
    set
    {
      _file = value;
      OnProprtyChanged();
    }
  }

  public bool IsBackground
  {
    get
    {
      return _isBackground;
    }
    set
    {
      _isBackground = value;
      OnProprtyChanged();
    }
  }

  public bool IsLooped
  {
    get
    {
      return _isLooped;
    }
    set
    {
      _isLooped = value;
      OnProprtyChanged();
    }
  }

  public ColorPalette Color
  {
    get
    {
      return _color;
    }
    set
    {
      _color = value;
      OnProprtyChanged();
    }
  }

  public IMediaPlayerViewModel Player
  {
    get
    {
      return _player;
    }
    set
    {
      _player = value;
      OnProprtyChanged();
    }
  }

  public bool IsPlayable
  {
    get
    {
      return _isPlayable;
    }
    set
    {
      _isPlayable = value;
      OnProprtyChanged();
    }
  }

  public IAsyncCommand<ITileViewModel> TapCommand { get; set; }

  public Action StopAllExcludingBackground { get; set; }

  public TileViewModel(ITapPlayerService tapPlayerService)
  {
    _tapPlayerService = tapPlayerService;

    TapCommand = new AsyncCommand<ITileViewModel>(Tap);
  }

  private Task Tap(ITileViewModel model)
  {
    if (!IsBackground)
    {
      // exclude the current tile, because the state of the MediaElement may not change immediately
      // and this can lead to incorrect determination of whether to start or stop playback
      _tapPlayerService.StopAllExcludingBackground(x => x != this);
    }

    if (Player == null)
    {
      return Task.CompletedTask;
    }

    if (Player.IsPlaying)
    {
      Player.Stop();
    }
    else
    {
      Player.Play();
    }

    return Task.CompletedTask;
  }
}
