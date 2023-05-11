namespace TapPlayer.Core.Dto.Projects;

public class GetProjectListItem
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public DateTimeOffset? LastLoadDate { get; set; }
}
