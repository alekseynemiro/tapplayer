using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TapPlayer.Core.Dto.Projects;
using TapPlayer.Core.Services.Projects;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Resources.Strings;
using TapPlayer.Maui.Services;
using static Android.Graphics.ColorSpace;

namespace TapPlayer.Maui.ViewModels;

public class ProjectEditViewModel : ViewModelBase, IProjectEditViewModel
{
  private readonly ILogger _logger;
  private readonly IServiceProvider _serviceProvider;
  private readonly IProjectService _projectService;
  private readonly INavigationService _navigationService;
  private readonly IDialogService _dialogService;

  private ObservableCollection<GridSize> _availableGridSizes = new ObservableCollection<GridSize>(Enum.GetValues<GridSize>());
  private Guid _projectId = Guid.Empty;
  private string _projectName = string.Empty;
  private GridSize _gridSize = GridSize.Grid3x3;
  private ObservableCollection<ITileViewModel> _tiles = new ObservableCollection<ITileViewModel>();
  private double _tilesGridWidth = -1;
  private double _tilesGridHeight = -1;
  private bool _showLoader = true;
  private bool _canSetGridSize = true;
  private bool _isLoaded = false;
  private bool _isSaving = false;
  private bool _isCreated = false;

  public ObservableCollection<GridSize> AvailableGridSizes
  {
    get
    {
      return _availableGridSizes;
    }
    set
    {
      _availableGridSizes = value;
      OnProprtyChanged();
    }
  }

  public Guid ProjectId
  {
    get
    {
      return _projectId;
    }
    set
    {
      _projectId = value;
      OnProprtyChanged();
      OnProprtyChanged(nameof(IsNew));
      OnProprtyChanged(nameof(Title));
    }
  }

  public string ProjectName
  {
    get
    {
      return _projectName;
    }
    set
    {
      _projectName = value;
      OnProprtyChanged();
    }
  }

  public GridSize GridSize
  {
    get
    {
      return _gridSize;
    }
    set
    {
      _gridSize = value;
      OnProprtyChanged();
    }
  }

  public ObservableCollection<ITileViewModel> Tiles
  {
    get
    {
      return _tiles;
    }
    set
    {
      _tiles = value;
      OnProprtyChanged();
    }
  }

  public double TilesGridWidth
  {
    get
    {
      return _tilesGridWidth;
    }
    set
    {
      _tilesGridWidth = value;
      OnProprtyChanged();
    }
  }

  public double TilesGridHeight
  {
    get
    {
      return _tilesGridHeight;
    }
    set
    {
      _tilesGridHeight = value;
      OnProprtyChanged();
    }
  }

  public bool ShowLoader
  {
    get
    {
      return _showLoader;
    }
    set
    {
      _showLoader = value;
      OnProprtyChanged();
    }
  }

  public bool CanSetGridSize
  {
    get
    {
      return _canSetGridSize;
    }
    set
    {
      _canSetGridSize = value;
      OnProprtyChanged();
    }
  }

  public string Title
  {
    get
    {
      return IsNew
        ? CommonStrings.NewProject
        : CommonStrings.Project;
    }
  }

  public bool IsNew
  {
    get
    {
      return ProjectId == Guid.Empty;
    }
  }

  public bool IsLoaded
  {
    get
    {
      return _isLoaded;
    }
    private set
    {
      _isLoaded = value;
      OnProprtyChanged();
    }
  }

  public bool IsSaving
  {
    get
    {
      return _isSaving;
    }
    private set
    {
      _isSaving = value;
      OnProprtyChanged();
    }
  }

  public bool IsCreated
  {
    get
    {
      return _isCreated;
    }
    private set
    {
      _isCreated = value;
      OnProprtyChanged();
    }
  }

  public IAsyncCommand<Guid> LoadCommand { get; init; }

  public IAsyncCommand SaveCommand { get; init; }

  public IAsyncCommand<ITileViewModel> TileEditCommand { get; init; }

  public IAsyncCommand CancelCommand { get; init; }

  public ICommand SelectGridSizeCommand { get; init; }

  public ProjectEditViewModel(
    ILogger<ProjectEditViewModel> logger,
    IServiceProvider serviceProvider,
    IProjectService projectService,
    INavigationService navigationService,
    IDialogService dialogService
  )
  {
    _logger = logger;
    _serviceProvider = serviceProvider;
    _projectService = projectService;
    _navigationService = navigationService;
    _dialogService = dialogService;

    LoadCommand = new AsyncCommand<Guid>(Load);
    SaveCommand = new AsyncCommand(Save);
    CancelCommand = new AsyncCommand(Cancel);
    TileEditCommand = new AsyncCommand<ITileViewModel>(TileEdit);

    InitDefault();
  }

  private async Task Load(Guid projectId)
  {
    _logger.LogDebug(nameof(Load) + " {ProjectId}", projectId);

    if (projectId == Guid.Empty)
    {
      throw new ArgumentOutOfRangeException(
        nameof(projectId),
        "The value must not be empty."
      );
    }

    IsLoaded = false;
    ShowLoader = true;

    WeakReferenceMessenger.Default.Send<IProjectEditViewModel>(this);

    var project = await _projectService.Get(projectId);

    Tiles.Clear();

    // immediately create the maximum number of tiles
    // this will not lose settings if the user changes the number of tiles
    int size = Enum.GetValues<GridSize>().Last().GetTotalCount();

    for (int i = 0; i < size; ++i)
    {
      var act = project.Acts.ElementAtOrDefault(i);
      var tile = _serviceProvider.GetRequiredService<ITileViewModel>();

      tile.Index = i;
      tile.Name = act?.Name;
      tile.File = new FileViewModel(act?.FilePath);
      tile.Color = act?.Color ?? ColorPalette.Color1;
      tile.IsBackground = act?.IsBackground == true;
      tile.IsLooped = act?.Play == PlayType.Loop;
      tile.TapCommand = TileEditCommand;
      tile.IsPlayable = false;

      Tiles.Add(tile);
    }

    ProjectId = project.Id;
    ProjectName = project.Name;
    GridSize = project.Size;

    IsLoaded = true;
    ShowLoader = false;

    WeakReferenceMessenger.Default.Send<IProjectEditViewModel>(this);
  }

  private async Task Save()
  {
    _logger.LogDebug(nameof(Save));

    if (string.IsNullOrWhiteSpace(ProjectName))
    {
      // TODO: Localization Service
      await _dialogService.Error(CommonStrings.ProjectNameIsRequired);

      return;
    }

    IsSaving = true;

    WeakReferenceMessenger.Default.Send<IProjectEditViewModel>(this);

    var newProjectId = Guid.Empty;

    if (ProjectId == Guid.Empty)
    {
      newProjectId = await _projectService.Create(new CreateProjectRequest
      {
        Name = ProjectName,
        Size = GridSize,
        Acts = Tiles
          .Take(GridSize.GetTotalCount())
          .Select(x => new CreateProjectRequestAct
          {
            Name = x.Name,
            Color = x.Color,
            FilePath = x.File?.FullPath,
            IsBackground = x.IsBackground,
            Play = x.IsLooped ? PlayType.Loop : PlayType.Once,
          }).ToList(),
      });

      IsCreated = true;
    }
    else
    {
      await _projectService.Update(new UpdateProjectRequest
      {
        Id = ProjectId,
        Name = ProjectName,
        Size = GridSize,
        Acts = Tiles
          .Take(GridSize.GetTotalCount())
          .Select(x => new UpdateProjectRequestAct
          {
            Name = x.Name,
            Color = x.Color,
            FilePath = x.File?.FullPath,
            IsBackground = x.IsBackground,
            Play = x.IsLooped ? PlayType.Loop : PlayType.Once,
          })
          .ToList(),
      });
    }

    IsSaving = false;

    if (IsCreated)
    {
      await _navigationService.OpenProject(newProjectId);
    }
    else
    {
      await _navigationService.CloseProjectEditor();
    }

    WeakReferenceMessenger.Default.Send<IProjectEditViewModel>(this);
  }

  private Task Cancel()
  {
    _logger.LogDebug(nameof(Cancel));

    return _navigationService.CloseProjectEditor();
  }

  private async Task TileEdit(ITileViewModel tile)
  {
    _logger.LogDebug(nameof(TileEdit) + " #{Index}", tile?.Index);

    var actEditPage = _serviceProvider.GetRequiredService<TileEditPage>();

    actEditPage.Model.SetCommand.Execute(tile);
    actEditPage.Model.Saved = TileSaved;

    await _navigationService.PushModalAsync(actEditPage);
  }

  private Task TileSaved(TileEditPageViewModelEventArgs e)
  {
    _logger.LogDebug(nameof(TileSaved) + " #{Index}", e.Index);

    var tile = Tiles[e.Index];

    tile.Name = e.Name;
    tile.File = new FileViewModel(e.FileFullPath);
    tile.Color = e.Color;
    tile.IsBackground = e.IsBackground;
    tile.IsLooped = e.PlayLoop;

    _logger.LogDebug(nameof(Tiles) + "." + nameof(ObservableCollectionExtensions.UpdateItem) + " #{Index}", e?.Index);

    Tiles.UpdateItem(tile);

    return Task.CompletedTask;
  }

  private void InitDefault()
  {
    _logger.LogDebug(nameof(InitDefault));

    IsLoaded = false;
    ShowLoader = true;

    Tiles.Clear();

    // immediately create the maximum number of tiles
    // this will not lose settings if the user changes the number of tiles
    int size = Enum.GetValues<GridSize>().Last().GetTotalCount();

    for (int i = 0; i < size; ++i)
    {
      var tile = _serviceProvider.GetRequiredService<ITileViewModel>();

      tile.Index = i;
      tile.Name = string.Empty;
      tile.File = null;
      tile.Color = ColorPalette.Color1;
      tile.TapCommand = TileEditCommand;
      tile.IsPlayable = false;

      Tiles.Add(tile);
    }

    ProjectId = Guid.Empty;
    ProjectName = string.Empty;
    GridSize = GridSize.Grid3x3;

    IsLoaded = true;
    ShowLoader = false;
  }
}
