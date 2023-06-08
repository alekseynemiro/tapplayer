using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Globalization;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class AppSettingsPageViewModel : ViewModelBase, IAppSettingsPageViewModel
{
  private readonly ILogger _logger;
  private readonly IAppSettingsService _appSettingsService;

  private AvailableLanguageViewModel _currentLanguage;

  private ObservableCollection<AvailableLanguageViewModel> _availableLanguages = new ObservableCollection<AvailableLanguageViewModel>
  {
    new AvailableLanguageViewModel(CultureInfo.GetCultureInfo("en-US")),
    new AvailableLanguageViewModel(CultureInfo.GetCultureInfo("ru-RU")),
  };

  public ObservableCollection<AvailableLanguageViewModel> AvailableLanguages
  {
    get => _availableLanguages;
  }

  public AvailableLanguageViewModel CurrentLanguage
  {
    get
    {
      return _currentLanguage;
    }
    set
    {
      _currentLanguage = value;
      OnProprtyChanged();
    }
  }

  public AsyncCommand SaveCommand { get; init; }

  public AsyncCommand CloseCommand { get; init; }

  public AppSettingsPageViewModel(
    ILogger<AppSettingsPageViewModel> logger,
    IAppSettingsService appSettingsService,
    INavigationService navigationService
  )
  {
    _logger = logger;
    _appSettingsService = appSettingsService;

#if DEBUG
    _logger.LogDebug(
      "AvailableLanguages: {Languages}",
      string.Join(", ", AvailableLanguages.Select(x => $"{x.Code} - {x.Name}"))
    );
#endif

    var currentCultureCode = _appSettingsService.Language != 0
      ? _appSettingsService.Language
      : CultureInfo.CurrentUICulture.LCID;

    CurrentLanguage = AvailableLanguages.FirstOrDefault(
      x => x.Code == currentCultureCode,
      AvailableLanguages.First()
    );

    _logger.LogInformation("Language: {CurrentLanguage}", CurrentLanguage?.Name);

    SaveCommand = new AsyncCommand(() =>
    {
      _logger.LogInformation(
        "SaveCommand: {CurrentLanguageName} ({CurrentLanguageCode})",
        CurrentLanguage?.Name,
        CurrentLanguage?.Code
      );

      appSettingsService.Language = CurrentLanguage.Code;

      return navigationService.PopAsync();
    });

    CloseCommand = new AsyncCommand(navigationService.PopAsync);
  }
}
