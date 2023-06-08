using System.Windows.Input;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class TileEditPageViewModel : ViewModelBase, ITileEditPageViewModel
{
  private string _name;
  private FileViewModel _file = new FileViewModel();
  private bool _playOnce;
  private bool _playLoop;
  private bool _isBackground;
  private ColorPalette _color;

  public int Index { get; set; }

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

  public bool PlayOnce
  {
    get
    {
      return _playOnce;
    }
    set
    {
      _playOnce = value;
      OnProprtyChanged();
    }
  }

  public bool PlayLoop
  {
    get
    {
      return _playLoop;
    }
    set
    {
      _playLoop = value;
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

  public IAsyncCommand SaveCommand { get; init; }

  public ICommand CancelCommand { get; init; }

  public ICommand<TileViewModel> SetCommand { get; init; }

  public Func<TileEditPageViewModelEventArgs, Task> Saved { get; set; }

  public TileEditPageViewModel(
    INavigationService navigationService
  )
  {
    SetCommand = new Command<TileViewModel>(Set);

    SaveCommand = new AsyncCommand(async () =>
    {
      await navigationService.PopModalAsync();
      await Saved?.Invoke(new TileEditPageViewModelEventArgs(
        Index,
        Name,
        File.FullPath,
        Color,
        IsBackground,
        PlayLoop
      ));
    });

    CancelCommand = new Command(async () =>
    {
      await navigationService.PopModalAsync();
    });
  }

  private void Set(TileViewModel tile)
  {
    Index = tile.Index;
    Name = tile.Name;
    Color = tile.Color;
    File = new FileViewModel(tile.File?.FullPath);
    IsBackground = tile.IsBackground;
    PlayLoop = tile.IsLooped;
    PlayOnce = !tile.IsLooped;
  }
}
