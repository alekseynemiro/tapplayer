namespace TapPlayer.Maui.Extensions;

internal class GridCreateEventArgs
{
  public int Column { get; }
  public int Row { get; }
  public int Index { get; }

  public GridCreateEventArgs(int column, int row, int index)
  {
    Column = column;
    Row = row;
    Index = index;
  }
}
