using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.Extensions;

internal static class GridSizeExtensions
{
  public static (int columns, int rows) GetColumnsAndRows(this GridSize gridSize)
  {
    switch (gridSize)
    {
      case GridSize.Grid4x4:
        return (4, 4);

      case GridSize.Grid3x4:
        return (3, 4);

      case GridSize.Grid3x3:
        return (3, 3);

      case GridSize.Grid2x3:
        return (2, 3);

      case GridSize.Grid2x2:
        return (2, 2);

      default:
        throw new NotImplementedException($"{gridSize} is not implemented.");
    }
  }

  public static int GetTotalCount(this GridSize gridSize)
  {
    var (columns, rows) = GetColumnsAndRows(gridSize);

    return columns * rows;
  }
}
