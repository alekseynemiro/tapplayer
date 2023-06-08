using System.Collections.ObjectModel;
using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.ViewModels;

public interface IMainPageViewModel
{
  string Title { get; }
  Guid ProjectId { get; }
  string ProjectName { get; }
  GridSize GridSize { get; }
  ObservableCollection<ITileViewModel> Tiles { get; }
  bool ShowCreateOrOpenProject { get; }
  bool ShowCreateProject { get; }
  bool ShowTiles { get; }
  bool ShowActivityIndicator { get; }

  IAsyncCommand InitCommand { get; }
  IAsyncCommand<Guid> LoadCommand { get; }
  IAsyncCommand OpenProjectCommand { get; }
  IAsyncCommand CreateProjectCommand { get; }

  void HideActivityIndicator();
}
