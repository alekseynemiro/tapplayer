using TapPlayer.Data.Enums;

namespace TapPlayer.Maui;

public class SelectColorEventArgs
{
  public ColorPalette Color { get; init; }

  public SelectColorEventArgs(ColorPalette color)
  {
    Color = color;
  }
}
