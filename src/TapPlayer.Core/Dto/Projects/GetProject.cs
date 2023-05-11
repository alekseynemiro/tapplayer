using TapPlayer.Data.Enums;

namespace TapPlayer.Core.Dto.Projects;

public class GetProject
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public GridSize Size { get; set; }
  public List<GetProjectAct> Acts { get; set; }

  public override string ToString()
  {
    return $"{Name} - {Size}";
  }
}
