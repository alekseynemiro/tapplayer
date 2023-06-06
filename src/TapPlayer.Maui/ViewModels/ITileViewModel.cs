using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.ViewModels;

public interface ITileViewModel
{
  int Index { get; set; }

  string Name { get; set; }

  FileViewModel File { get; set; }

  PlayType PlayType { get; set; }

  bool IsBackground { get; set; }

  ColorPalette Color { get; set; }

  IMediaPlayerViewModel Player { get; set; }

  bool IsPlayable { get; set; }

  IAsyncCommand<ITileViewModel> TapCommand { get; set; }

  Action StopAllExcludingBackground { get; }
}
