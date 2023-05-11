using Plugin.Maui.Audio;
using System.Windows.Input;
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

  Stream FileStream { get; set; }

  IAudioPlayer Player { get; set; }

  ICommand<IProjectEditViewModel> EditCommand { get; }

  ICommand TapCommand { get; }

  Action StopAllExcludingBackground { get; }
}
