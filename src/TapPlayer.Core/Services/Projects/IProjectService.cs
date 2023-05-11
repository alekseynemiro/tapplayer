using TapPlayer.Core.Dto.Projects;

namespace TapPlayer.Core.Services.Projects;

public interface IProjectService
{
  Task<GetProject> Get(Guid id);

  Task<Guid> Create(CreateProjectRequest request);

  Task Update(UpdateProjectRequest request);

  Task Delete(Guid id);
}
