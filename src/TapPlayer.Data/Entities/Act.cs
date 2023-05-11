using TapPlayer.Data.Enums;

namespace TapPlayer.Data.Entities;

public class Act
{
  public string Name { get; set; }
  public string FilePath { get; set; }
  public PlayType Play { get; set; }
  public bool IsBackground { get; set; } // Do not stop
  public ColorPalette Color { get; set; }
}
