using System.Globalization;
using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.Converters;

internal class GridSizeConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value?.GetType() != typeof(GridSize))
    {
      throw new InvalidCastException();
    }

    var gridSize = Enum.Parse<GridSize>(value.ToString());

    switch (gridSize)
    {
      case GridSize.Grid4x4:
        return "4x4";

      case GridSize.Grid3x4:
        return "3x4";

      case GridSize.Grid3x3:
        return "3x3";

      case GridSize.Grid2x3:
        return "2x3";

      case GridSize.Grid2x2:
        return "2x2";

      default:
        throw new NotImplementedException($"{gridSize} is not implemented.");
    }
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
