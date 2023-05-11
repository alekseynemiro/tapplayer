using TapPlayer.Data.Enums;

namespace TapPlayer.Data.Entities;

public class Scene
{
  public GridSize Size { get; set; }
  public List<Act> Acts { get; set; }
}
