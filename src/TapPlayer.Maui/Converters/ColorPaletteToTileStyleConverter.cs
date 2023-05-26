using System.Globalization;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;

namespace TapPlayer.Maui.Converters;

internal class ColorPaletteToTileStyleConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
    {
      return null;
    }

    if (value.GetType() != typeof(ColorPalette))
    {
      throw new InvalidCastException();
    }

    var color = Enum.Parse<ColorPalette>(value.ToString());
    var style = Application.Current.Resources.FindResource($"Tile{(int)color + 1}");

    return style;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
