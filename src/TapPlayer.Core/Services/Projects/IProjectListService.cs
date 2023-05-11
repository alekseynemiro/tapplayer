using TapPlayer.Core.Dto.Projects;

namespace TapPlayer.Core.Services.Projects;

public interface IProjectListService
{
  Task<GetProjectList> GetAll();
}
