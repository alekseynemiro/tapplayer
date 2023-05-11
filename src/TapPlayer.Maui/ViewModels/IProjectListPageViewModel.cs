using System.Collections.ObjectModel;

namespace TapPlayer.Maui.ViewModels;

public interface IProjectListPageViewModel
{
  ObservableCollection<IProjectListItemViewModel> Projects { get; set; }

  bool ShowRefreshing { get; set; }

  bool IsLoaded { get; set; }

  bool ShowNoProjects { get; set; }

  IAsyncCommand LoadCommand { get; }
  IAsyncCommand CreateCommand { get; }
}
