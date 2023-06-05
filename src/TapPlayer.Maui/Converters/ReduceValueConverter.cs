using System.Globalization;

namespace TapPlayer.Maui.Converters;

internal class ReduceValueConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    var valueInt = System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
    var parameterInt = System.Convert.ToInt32(parameter, CultureInfo.InvariantCulture);
    var result = valueInt - parameterInt;

    return result;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
  }
}
