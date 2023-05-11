using System.Globalization;
using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.Converters;

internal class ColorPaletteConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    var color = ColorPalette.Color1;

    if (value != null)
    {
      color = Enum.Parse<ColorPalette>(value.ToString());
    }

    return $"Color{(int)color + 1}";
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value != null)
    {
      return Enum.Parse<ColorPalette>(value.ToString());
    }

    throw null;
  }
}
