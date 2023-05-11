using TapPlayer.Core.Dto.Projects;
using TapPlayer.Data;

namespace TapPlayer.Core.Services.Projects;

public class ProjectListService : IProjectListService
{
  private readonly IJsonStorage _jsonStorage;

  public ProjectListService(IJsonStorage jsonStorage)
  {
    _jsonStorage = jsonStorage;
  }

  public async Task<GetProjectList> GetAll()
  {
    var projectList = await _jsonStorage.LoadProjectList();

    return new GetProjectList
    {
      Items = projectList.Items.Select(x => new GetProjectListItem
      {
        Id = x.Id,
        Name = x.Name,
        LastLoadDate = x.LastLoadDate,
      }).ToList(),
    };
  }
}
