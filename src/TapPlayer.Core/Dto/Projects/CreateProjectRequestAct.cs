using TapPlayer.Data.Enums;

namespace TapPlayer.Core.Dto.Projects;

public class CreateProjectRequestAct
{
  public string Name { get; set; }
  public string FilePath { get; set; }
  public PlayType Play { get; set; }
  public bool IsBackground { get; set; }
  public ColorPalette Color { get; set; }
}
