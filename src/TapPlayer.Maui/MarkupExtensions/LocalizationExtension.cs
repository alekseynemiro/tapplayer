using Microsoft.Extensions.Localization;
using TapPlayer.Maui.Resources.Strings;

namespace TapPlayer.Maui.MarkupExtensions;

[ContentProperty(nameof(Key))]
internal class LocalizationExtension : IMarkupExtension
{
  private IStringLocalizer<CommonStrings> _stringLocalizer;

  public string Key { get; set; } = string.Empty;

  public LocalizationExtension()
  {
    _stringLocalizer = MauiProgram.ServiceProvider.GetRequiredService<IStringLocalizer<CommonStrings>>();
  }

  public object ProvideValue(IServiceProvider serviceProvider)
  {
    return _stringLocalizer[Key];
  }

  object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
