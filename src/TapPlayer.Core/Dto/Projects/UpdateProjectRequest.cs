using TapPlayer.Data.Enums;

namespace TapPlayer.Core.Dto.Projects;

public class UpdateProjectRequest
{
  public Guid Id { get; set; }
  public GridSize Size { get; set; }
  public string Name { get; set; }
  public List<UpdateProjectRequestAct> Acts { get; set; }
}
