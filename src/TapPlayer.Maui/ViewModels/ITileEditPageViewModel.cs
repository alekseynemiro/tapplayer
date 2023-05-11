using System.Windows.Input;
using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.ViewModels;

public interface ITileEditPageViewModel
{
  int Index { get; set; }

  string Name { get; set; }

  FileViewModel File { get; set; }

  bool PlayOnce { get; set; }

  bool PlayLoop { get; set; }

  bool IsBackground { get; set; }

  ColorPalette Color { get; set; }

  IAsyncCommand SaveCommand { get; }

  ICommand CancelCommand { get; }

  ICommand<TileViewModel> SetCommand { get; }

  Action<TileEditPageViewModel> Saved { get; set; }
}
