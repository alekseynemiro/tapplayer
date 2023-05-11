using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using TapPlayer.Core.Services.Projects;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class ProjectListPageViewModel : ViewModelBase, IProjectListPageViewModel
{
  private readonly IServiceProvider _serviceProvider;
  private readonly IProjectListService _proectListService;
  private readonly INavigationService _navigationService;

  private bool _isLoaded = false;
  private bool _showRefreshing = false;
  private bool _showNoProjects = false;

  public ObservableCollection<IProjectListItemViewModel> Projects { get; set; } = new ObservableCollection<IProjectListItemViewModel>();

  public bool ShowRefreshing
  {
    get
    {
      return _showRefreshing;
    }
    set
    {
      _showRefreshing = value;
      OnProprtyChanged();
    }
  }

  public bool IsLoaded
  {
    get
    {
      return _isLoaded;
    }
    set
    {
      _isLoaded = value;
      OnProprtyChanged();
    }
  }

  public bool ShowNoProjects
  {
    get
    {
      return _showNoProjects;
    }
    set
    {
      _showNoProjects = value;
      OnProprtyChanged();
    }
  }

  public IAsyncCommand LoadCommand { get; init; }
  public IAsyncCommand CreateCommand { get; init; }

  public ProjectListPageViewModel(
    IServiceProvider serviceProvider,
    IProjectListService proectListService,
    INavigationService navigationService
  )
  {
    _serviceProvider = serviceProvider;
    _navigationService = navigationService;
    _proectListService = proectListService;

    LoadCommand = new AsyncCommand(Load);
    CreateCommand = new AsyncCommand(Create);

    WeakReferenceMessenger.Default.Register<IProjectEditViewModel>(
      this,
      async (r, m) =>
      {
        if (m.IsCreated)
        {
          await LoadCommand.ExecuteAsync();
        }
      }
    );
  }

  private async Task Load()
  {
    IsLoaded = false;
    ShowRefreshing = true;
    ShowNoProjects = false;

    Projects.Clear();

    var projectList = await _proectListService.GetAll();

    foreach (var item in projectList.Items)
    {
      var project = _serviceProvider.GetRequiredService<IProjectListItemViewModel>();

      project.SetCommand.Execute(item);
      project.Changed = Load;

      Projects.Add(project);
    }

    ShowNoProjects = Projects.Count == 0;
    ShowRefreshing = false;
    IsLoaded = true;

    WeakReferenceMessenger.Default.Send<IProjectListPageViewModel>(this);
  }

  private Task Create()
  {
    return _navigationService.CreateProject();
  }
}
