using System.Globalization;

namespace TapPlayer.Maui.Converters;

internal class EmptyValueToBoolConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
    {
      return true;
    }

    if (value.GetType() == typeof(string))
    {
      return string.IsNullOrWhiteSpace(value.ToString());
    }

    throw new NotSupportedException($"{value.GetType()} is not supported.");
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}