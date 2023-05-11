using TapPlayer.Core.Dto.Projects;
using TapPlayer.Core.Services.Projects;
using TapPlayer.Maui.Resources.Strings;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class ProjectListItemViewModel : ViewModelBase, IProjectListItemViewModel
{
  private readonly IProjectService _proectService;
  private readonly INavigationService _navigationService;
  private readonly IDialogService _dialogService;

  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public DateTimeOffset? LastLoadDate { get; private set; }

  public ICommand<GetProjectListItem> SetCommand { get; init; }
  public IAsyncCommand OpenCommand { get; init; }
  public IAsyncCommand EditCommand { get; init; }
  public IAsyncCommand DeleteCommand { get; init; }

  public Func<Task> Changed { get; set; }

  public ProjectListItemViewModel(
    IProjectService proectService,
    INavigationService navigationService,
    IDialogService dialogService
  )
  {
    _proectService = proectService;
    _navigationService = navigationService;
    _dialogService = dialogService;

    SetCommand = new Command<GetProjectListItem>(Set);
    OpenCommand = new AsyncCommand(Open);
    EditCommand = new AsyncCommand(Edit);
    DeleteCommand = new AsyncCommand(Delete);
  }

  private void Set(GetProjectListItem project)
  {
    Id = project.Id;
    Name = project.Name;
    LastLoadDate = project.LastLoadDate;
  }

  private Task Open()
  {
    return _navigationService.OpenProject(Id);
  }

  protected Task Edit()
  {
    return _navigationService.ProjectSettings(Id);
  }

  protected async Task Delete()
  {
    // TODO: Localization service
    var confirmed = await _dialogService.Confirm(
      string.Format(CommonStrings.AreYouSureYouWantToDeleteTheProject, Name),
      accept: CommonStrings.Yes,
      cancel: CommonStrings.Cancel
    );

    if (confirmed)
    {
      await _proectService.Delete(Id);
      await Changed?.Invoke();
    }
  }
}
