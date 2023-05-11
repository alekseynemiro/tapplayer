using TapPlayer.Data.Enums;

namespace TapPlayer.Core.Dto.Projects;

public class CreateProjectRequest
{
  public GridSize Size { get; set; }
  public string Name { get; set; }
  public List<CreateProjectRequestAct> Acts { get; set; }
}
