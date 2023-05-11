using TapPlayer.Data.Entities;

namespace TapPlayer.Data;

public interface IJsonStorage
{
  Task<ProjectList> LoadProjectList();

  Task<Project> LoadProject(Guid projectId);

  Task SaveProject(Project project);

  Task DeleteProject(Guid projectId);
}
