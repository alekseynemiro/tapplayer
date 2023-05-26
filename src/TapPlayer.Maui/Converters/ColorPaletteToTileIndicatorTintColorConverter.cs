using Microsoft.Maui.Animations;
using System.Globalization;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;

namespace TapPlayer.Maui.Converters;

internal class ColorPaletteToTileIndicatorTintColorConverter : IValueConverter
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

    var index = (int)Enum.Parse<ColorPalette>(value.ToString());
    var color = (Color)Application.Current.Resources.FindResource($"Color{index + 1}");

    return Color.FromRgb(255, 255, 255).Lerp(color, 0.5);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
