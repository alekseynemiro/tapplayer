using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.ViewModels;

public class TileEditPageViewModelEventArgs : EventArgs
{
  public int Index { get; init; }
  public string Name { get; init; }
  public string FileFullPath { get; init; }
  public ColorPalette Color { get; init; }
  public bool IsBackground { get; init; }
  public bool PlayLoop { get; init; }

  public TileEditPageViewModelEventArgs(
    int index,
    string name,
    string fileFullPath,
    ColorPalette color,
    bool isBackgroud,
    bool playLoop
  )
  {
    Index = index;
    Name = name;
    FileFullPath = fileFullPath;
    Color = color;
    IsBackground = isBackgroud;
    PlayLoop = playLoop;
  }
}
