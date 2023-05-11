using TapPlayer.Core.Dto.Projects;
using TapPlayer.Data;
using TapPlayer.Data.Entities;

namespace TapPlayer.Core.Services.Projects;

public class ProjectService : IProjectService
{
  private readonly IJsonStorage _jsonStorage;
  private readonly IDateTimeService _dateTimeService;

  public ProjectService(
    IJsonStorage jsonStorage,
    IDateTimeService dateTimeService
  )
  {
    _jsonStorage = jsonStorage;
    _dateTimeService = dateTimeService;
  }

  public async Task<GetProject> Get(Guid id)
  {
    var project = await _jsonStorage.LoadProject(id);
    var scene = project.Scenes.Single();

    return new GetProject
    {
      Id = project.Id,
      Name = project.Name,
      Size = scene.Size,
      Acts = scene.Acts.Select(x => new GetProjectAct
      {
        Name = x.Name,
        Color = x.Color,
        FilePath = x.FilePath,
        IsBackground = x.IsBackground,
        Play = x.Play,
      }).ToList(),
    };
  }

  public async Task<Guid> Create(CreateProjectRequest request)
  {
    var project = new Project
    {
      Id = Guid.NewGuid(),
      CreatedDate = _dateTimeService.UtcNow,
      Name = request.Name,
      Scenes = new List<Scene>
      {
        new Scene
        {
          Size = request.Size,
          Acts = request.Acts.Select(x => new Act
          {
            Name = x.Name,
            FilePath = x.FilePath,
            Color = x.Color,
            IsBackground = x.IsBackground,
            Play = x.Play,
          }).ToList(),
        },
      },
    };

    await _jsonStorage.SaveProject(project);

    return project.Id;
  }

  public async Task Update(UpdateProjectRequest request)
  {
    var project = await _jsonStorage.LoadProject(request.Id);
    var scene = project.Scenes.Single();

    project.Name = request.Name;

    scene.Size = request.Size;
    scene.Acts = request.Acts.Select(x => new Act
    {
      Name = x.Name,
      FilePath = x.FilePath,
      Color = x.Color,
      IsBackground = x.IsBackground,
      Play = x.Play,
    }).ToList();

    await _jsonStorage.SaveProject(project);
  }

  public Task Delete(Guid id)
  {
    return _jsonStorage.DeleteProject(id);
  }
}
