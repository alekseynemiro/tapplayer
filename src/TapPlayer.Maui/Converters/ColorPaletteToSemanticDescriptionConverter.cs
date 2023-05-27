using System.Globalization;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Resources.Strings;

namespace TapPlayer.Maui.Converters;

internal class ColorPaletteToSemanticDescriptionConverter : IValueConverter
{
  private static readonly List<string> ColorNames = new List<string>
  {
    "Violet",
    "Red",
    "Yellow",
    "Green",
    "BlueGreen",
    "Blue",
  };

  private static readonly List<string> ColorBrightnessNames = new List<string>
  {
    "Light",
    "Light50",
    "",
    "Dark25",
    "Dark50",
    "Dark",
  };

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

    var color = (int)Enum.Parse<ColorPalette>(value.ToString()) + 1;
    var group = (int)Math.Ceiling(color / 6.0) - 1;
    var brightness = (color % 6 == 0 ? 6 : color % 6) - 1;
    var key = $"Color{ColorNames[group]}{ColorBrightnessNames[brightness]}";

    return CommonStrings.ResourceManager.GetString(key);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
