namespace TapPlayer.Data.Entities;

public class Project
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public List<Scene> Scenes { get; set; }
  public DateTimeOffset CreatedDate { get; set; }
}