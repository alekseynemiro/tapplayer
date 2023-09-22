using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using TapPlayer.Core.Services.Projects;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class MainPageViewModel : ViewModelBase, IMainPageViewModel, IDisposable
{
  private readonly ILogger _logger;
  private readonly IServiceProvider _serviceProvider;
  private readonly IProjectService _projectService;
  private readonly IProjectListService _projectListService;
  private readonly ITapPlayerService _tapPlayerService;
  private readonly IAppSettingsService _appSettingsService;
  private readonly IDialogService _dialogService;
  private readonly IToastNotificationService _toastNotificationService;

  private Guid _projectId = Guid.Empty;

  private string _title = string.Empty;
  private string _projectName = string.Empty;
  private GridSize _gridSize = GridSize.Grid3x3;
  private ObservableCollection<ITileViewModel> _tiles = new ObservableCollection<ITileViewModel>();
  private bool _showCreateOrOpenProject = false;
  private bool _showCreateProject = false;
  private bool _showTiles = false;
  private bool _showActivityIndicator = false;
  private int _projectCount = 0;
  private bool _disposed = false;

  public string Title
  {
    get
    {
      return _title;
    }
    set
    {
      _title = value;
      OnProprtyChanged();
    }
  }

  public Guid ProjectId
  {
    get
    {
      return _projectId;
    }
    private set
    {
      _projectId = value;
      OnProprtyChanged();
    }
  }

  public string ProjectName
  {
    get
    {
      return _projectName;
    }
    private set
    {
      _projectName = value;
      OnProprtyChanged();
      Title = value;
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
    private set
    {
      _tiles = value;
      OnProprtyChanged();
    }
  }

  public bool ShowCreateOrOpenProject
  {
    get
    {
      return _showCreateOrOpenProject;
    }
    private set
    {
      _showCreateOrOpenProject = value;
      OnProprtyChanged();
    }
  }

  public bool ShowCreateProject
  {
    get
    {
      return _showCreateProject;
    }
    private set
    {
      _showCreateProject = value;
      OnProprtyChanged();
    }
  }

  public bool ShowTiles
  {
    get
    {
      return _showTiles;
    }
    private set
    {
      _showTiles = value;
      OnProprtyChanged();
    }
  }

  public bool ShowActivityIndicator
  {
    get
    {
      return _showActivityIndicator;
    }
    private set
    {
      _showActivityIndicator = value;
      OnProprtyChanged();
    }
  }

  public int ProjectCount
  {
    get
    {
      return _projectCount;
    }
    set
    {
      _projectCount = value;
      OnProprtyChanged();
    }
  }

  public IAsyncCommand InitCommand { get; init; }

  public IAsyncCommand<Guid> LoadCommand { get; init; }

  public IAsyncCommand OpenProjectCommand { get; init; }

  public IAsyncCommand CreateProjectCommand { get; init; }

  public MainPageViewModel(
    ILogger<MainPageViewModel> logger,
    IServiceProvider serviceProvider,
    IProjectService projectService,
    IProjectListService projectListService,
    INavigationService navigationService,
    ITapPlayerService tapPlayerService,
    IAppSettingsService appSettingsService,
    IDialogService dialogService,
    IToastNotificationService toastNotificationService
  )
  {
    _logger = logger;
    _serviceProvider = serviceProvider;
    _projectService = projectService;
    _projectListService = projectListService;
    _tapPlayerService = tapPlayerService;
    _appSettingsService = appSettingsService;
    _dialogService = dialogService;
    _toastNotificationService = toastNotificationService;

    InitCommand = new AsyncCommand(Init);
    LoadCommand = new AsyncCommand<Guid>(Load);
    CreateProjectCommand = new AsyncCommand(navigationService.CreateProject);
    OpenProjectCommand = new AsyncCommand(navigationService.ProjectList);

    WeakReferenceMessenger.Default.Register<IActiveProjectService>(
      this,
      async (r, m) =>
      {
        if (!m.HasProject)
        {
          await Init();
        }
      }
    );

    WeakReferenceMessenger.Default.Register<IProjectEditViewModel>(
      this,
      async (r, m) =>
      {
        if (m.IsCreated && ProjectCount <= 0)
        {
          await Init();
        }
      }
    );

    WeakReferenceMessenger.Default.Register<IProjectListPageViewModel>(
      this,
      async (r, m) =>
      {
        if (m.IsLoaded && m.ShowNoProjects && ProjectCount > 0)
        {
          await Init();
        }
      }
    );
  }

  public void Dispose()
  {
    if (_disposed)
    {
      return;
    }

    WeakReferenceMessenger.Default.Unregister<IActiveProjectService>(this);
    WeakReferenceMessenger.Default.Unregister<IProjectEditViewModel>(this);
    WeakReferenceMessenger.Default.Unregister<IProjectListPageViewModel>(this);

    _disposed = true;
  }

  public void HideActivityIndicator()
  {
    ShowActivityIndicator = false;
  }

  private async Task Init()
  {
    try
    {
      ShowStartLoading();

      var projects = await _projectListService.GetAll();

      ProjectCount = projects.Items.Count;
      ShowCreateProject = projects.Items.Count == 0;
      ShowCreateOrOpenProject = !ShowCreateProject && projects.Items.Count > 0;

      ProjectName = string.Empty;
      Tiles.Clear();
    }
    finally
    {
      HideActivityIndicator();
    }
  }

  private async Task Load(Guid projectId)
  {
    try
    {
      ShowStartLoading();

      await _tapPlayerService.DisposeAll();

      Tiles.Clear();

      var project = await _projectService.Get(projectId);

      int size = project.Size.GetTotalCount();

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

        Tiles.Add(tile);
      }

      ProjectId = project.Id;
      ProjectName = project.Name;
      GridSize = project.Size;

      await _tapPlayerService.Set(Tiles);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error opening project {ProjectId}.", projectId);
      _appSettingsService.LastProjectId = Guid.Empty;
      HideActivityIndicator();
      await _toastNotificationService.Error($"Failed to open project: \"{ex.Message}\".");
    }
    finally
    {
      ShowLoadResult();
    }
  }

  private void ShowStartLoading()
  {
    ShowActivityIndicator = true;
    ShowTiles = ShowCreateProject = ShowCreateOrOpenProject = false;
  }

  private void ShowLoadResult()
  {
    ShowCreateOrOpenProject = Tiles.Count == 0;
    ShowTiles = Tiles.Count > 0;
  }
}
