using System.Windows.Input;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class TileViewModel : ViewModelBase, ITileViewModel
{
  private readonly IMediaService _mediaService;
  private readonly ITapPlayerService _tapPlayerService;
  private readonly IDialogService _dialogService;

  private int _index;
  private string _name;
  private FileViewModel _file;
  private PlayType _playType;
  private bool _isBackground;
  private ColorPalette _color;
  private IMediaPlayerViewModel _player;

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

  public PlayType PlayType
  {
    get
    {
      return _playType;
    }
    set
    {
      _playType = value;
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

  public ICommand<IProjectEditViewModel> EditCommand { get; init; }

  public ICommand TapCommand { get; init; }

  public Action StopAllExcludingBackground { get; set; }

  public TileViewModel(
    IMediaService mediaService,
    ITapPlayerService tapPlayerService,
    IDialogService dialogService
  )
  {
    _mediaService = mediaService;
    _tapPlayerService = tapPlayerService;
    _dialogService = dialogService;

    TapCommand = new Command(Tap);
    EditCommand = new Command<IProjectEditViewModel>(x =>
    {
      x.TileEditCommand.Execute(this);
    });
  }

  private void Tap()
  {
    var play = Player?.IsPlaying != true;

    if (!IsBackground)
    {
      _tapPlayerService.StopAllExcludingBackground();
    }

    if (!string.IsNullOrWhiteSpace(File?.FullPath))
    {
      if (Player == null)
      {
        Player = _mediaService.CreatePlayer(File.FullPath);

        if (PlayType == PlayType.Loop)
        {
          Player.Loop = true;
        }
      }

      if (play)
      {
        TryToPlay();
      }
      else
      {
        Player.Stop();
      }
    }
  }

  private void TryToPlay()
  {
    try
    {
      Player.Play();
    }
    catch (Exception ex)
    {
      _dialogService.Error($"Failed to play file  \"{File.FullPath}\": {ex.Message}");
    }
  }
}
