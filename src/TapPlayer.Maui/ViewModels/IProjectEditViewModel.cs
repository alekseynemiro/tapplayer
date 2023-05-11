using System.Collections.ObjectModel;
using System.Windows.Input;
using TapPlayer.Data.Enums;

namespace TapPlayer.Maui.ViewModels;

public interface IProjectEditViewModel
{
  ObservableCollection<GridSize> AvailableGridSizes { get; set; }

  Guid ProjectId { get; set; }

  string ProjectName { get; set; }

  GridSize GridSize { get; set; }

  ObservableCollection<ITileViewModel> Tiles { get; set; }

  double TilesGridWidth { get; set; }

  double TilesGridHeight { get; set; }

  bool ShowRefreshing { get; set; }

  string Title { get; }

  bool IsNew { get; }

  bool IsLoaded { get; }

  bool IsSaving { get; }

  bool IsCreated { get; }

  IAsyncCommand<Guid> LoadCommand { get; }

  IAsyncCommand SaveCommand { get; }

  IAsyncCommand<ITileViewModel> TileEditCommand { get; }

  IAsyncCommand CancelCommand { get; }

  ICommand SelectGridSizeCommand { get; }
}
