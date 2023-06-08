using System.Globalization;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Converters;

internal class TileViewModelToMediaElementSourceConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    var tile = (ITileViewModel)value;

    if (
      tile == null
      || string.IsNullOrWhiteSpace(tile.File?.FullPath)
      || !tile.IsPlayable
    )
    {
      return null;
    }

    return tile.File.FullPath;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
