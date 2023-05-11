using TapPlayer.Data.Enums;

namespace TapPlayer.Core.Dto.Projects;

public class GetProjectAct
{
  public string Name { get; set; }
  public string FilePath { get; set; }
  public PlayType Play { get; set; }
  public bool IsBackground { get; set; }
  public ColorPalette Color { get; set; }

  public override string ToString()
  {
    return $"{Name}: {Play} \"{FilePath}\"{(IsBackground ? " in background" : string.Empty)}";
  }
}
