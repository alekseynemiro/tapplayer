namespace TapPlayer.Data.Entities;

public class ProjectListItem
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public DateTimeOffset? LastLoadDate { get; set; }
}
