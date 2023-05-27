using System.Globalization;

namespace TapPlayer.Maui.Converters;

internal class ReduceValueConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
    {
      return value;
    }

    if (System.Convert.ToInt32(value, CultureInfo.InvariantCulture) < 0)
    {
      return value;
    }

    var result = System.Convert.ToInt32(value, CultureInfo.InvariantCulture)
       - System.Convert.ToInt32(parameter, CultureInfo.InvariantCulture);

    return result;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
  }
}