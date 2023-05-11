using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.Extensions;

internal static class GridExtensions
{
  public static void Create(this Grid grid, GridSize gridSize, Func<GridCreateEventArgs, IView> getCellView = null)
  {
    var (columns, rows) = gridSize.GetColumnsAndRows();

    grid.ColumnDefinitions.Clear();
    grid.RowDefinitions.Clear();
    grid.Clear();

    for (int i = 0; i < columns; ++i)
    {
      grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
    }

    for (int i = 0; i < rows; ++i)
    {
      grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
    }

    for (int column = 0; column < columns; ++column)
    {
      for (int row = 0; row < rows; ++row)
      {
        int index = row + column + (row > 0 ? (columns - 1) * row : 0);

        grid.Add(
          getCellView.Invoke(new GridCreateEventArgs(column, row, index)),
          column,
          row
        );
      }
    }
  }
}
