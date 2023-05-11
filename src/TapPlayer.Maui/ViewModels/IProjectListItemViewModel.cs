using TapPlayer.Core.Dto.Projects;

namespace TapPlayer.Maui.ViewModels;

public interface IProjectListItemViewModel
{
  Guid Id { get; }
  string Name { get; }
  DateTimeOffset? LastLoadDate { get; }

  ICommand<GetProjectListItem> SetCommand { get; }
  IAsyncCommand OpenCommand { get; }
  IAsyncCommand EditCommand { get; }
  IAsyncCommand DeleteCommand { get; }

  Func<Task> Changed { get; set; }
}
