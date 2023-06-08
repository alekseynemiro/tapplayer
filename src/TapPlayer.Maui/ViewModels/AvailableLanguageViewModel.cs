using System.Globalization;

namespace TapPlayer.Maui.ViewModels;

public class AvailableLanguageViewModel
{
  public int Code { get; set; }
  public string Name { get; set; }

  public AvailableLanguageViewModel(CultureInfo cultureInfo)
  {
    Code = cultureInfo.LCID;
    Name = cultureInfo.NativeName;
  }
}
