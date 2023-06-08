using System.Collections.ObjectModel;

namespace TapPlayer.Maui.ViewModels;

public interface IAppSettingsPageViewModel
{
  ObservableCollection<AvailableLanguageViewModel> AvailableLanguages { get; }
  AvailableLanguageViewModel CurrentLanguage { get; set; }

  AsyncCommand SaveCommand { get; }
  AsyncCommand CloseCommand { get; }
}
