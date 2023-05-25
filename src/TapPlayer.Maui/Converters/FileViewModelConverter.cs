using System.Globalization;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Converters;

internal class FileViewModelConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    var file = (FileViewModel)value;

    if (string.IsNullOrWhiteSpace(file?.FullPath))
    {
      return null;
    }

    if (parameter?.ToString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
    {
      return file.FullPath;
    }

    return file.Name;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
