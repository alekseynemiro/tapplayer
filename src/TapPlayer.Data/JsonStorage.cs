using System.Text;
using System.Text.Json;
using TapPlayer.Data.Entities;

namespace TapPlayer.Data;

public class JsonStorage : IJsonStorage
{
  private readonly IJsonStorageConfig _jsonStorageConfig;

  public JsonStorage(IJsonStorageConfig jsonStorageConfig)
  {
    _jsonStorageConfig = jsonStorageConfig;
  }

  public async Task<Project> LoadProject(Guid projectId)
  {
    EnsureIsStorageDirectoryExists();

    if (projectId == Guid.Empty)
    {
      throw new ArgumentOutOfRangeException(nameof(projectId));
    }

    var projectList = await LoadProjectList();
    var projectListItem = projectList.Items.SingleOrDefault(x => x.Id == projectId);

    string filePath = GetProjectFilePath(projectId);

    using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

    // TODO: Research. There is a problem with asynchronous methods.
    var result = JsonSerializer.Deserialize<Project>(stream);

    // TODO: Use service instead of DateTimeOffset.UtcNow
    projectListItem.LastLoadDate = DateTimeOffset.UtcNow;

    return result;
  }

  public async Task SaveProject(Project project)
  {
    EnsureIsStorageDirectoryExists();

    if (project.Id == Guid.Empty)
    {
      throw new ArgumentOutOfRangeException(nameof(project.Id));
    }

    var projectList = await LoadProjectList();
    var projectListItem = projectList.Items.SingleOrDefault(x => x.Id == project.Id);

    string filePath = GetProjectFilePath(project.Id);

    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
    {
      await JsonSerializer.SerializeAsync(
        stream,
        project
      );
    }

    if (projectListItem == null)
    {
      projectListItem = new ProjectListItem
      {
        Id = project.Id,
        Name = project.Name,
      };

      projectList.Items.Add(projectListItem);
    }
    else
    {
      projectListItem.Name = project.Name;
      // TODO: Use service instead of DateTimeOffset.UtcNow
      projectListItem.LastLoadDate = DateTimeOffset.UtcNow;
    }

    await SaveProjectList(projectList);

    // TODO: What about transactions?
  }

  public async Task DeleteProject(Guid projectId)
  {
    EnsureIsStorageDirectoryExists();

    if (projectId == Guid.Empty)
    {
      throw new ArgumentOutOfRangeException(nameof(projectId));
    }

    var projectList = await LoadProjectList();
    var projectListItem = projectList.Items.SingleOrDefault(x => x.Id == projectId);

    string filePath = GetProjectFilePath(projectId);

    File.Delete(filePath);

    if (projectListItem != null)
    {
      projectList.Items.Remove(projectListItem);
      await SaveProjectList(projectList);
    }
  }

  public async Task<ProjectList> LoadProjectList()
  {
    EnsureIsStorageDirectoryExists();

    string filePath = GetProjectListFilePath();

    // TODO: Use service instead of File.Exists:
    if (!File.Exists(filePath))
    {
      return new ProjectList
      {
        Items = new List<ProjectListItem>(),
      };
    }

    using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

    return await JsonSerializer.DeserializeAsync<ProjectList>(stream);
  }

  private async Task SaveProjectList(ProjectList projectList)
  {
    EnsureIsStorageDirectoryExists();

    string filePath = GetProjectListFilePath();

    using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

    await JsonSerializer.SerializeAsync(
      stream,
      projectList
    );
  }

  private string GetProjectFilePath(Guid projectId)
  {
    // TODO: Use service instead of Path.Combine:
    return Path.Combine(_jsonStorageConfig.StoragePath, $"{projectId}{_jsonStorageConfig.ProjectFileExtension}");
  }

  private string GetProjectListFilePath()
  {
    // TODO: Use service instead of Path.Combine:
    return Path.Combine(_jsonStorageConfig.StoragePath, _jsonStorageConfig.ProjectListFileName);
  }

  private void EnsureIsStorageDirectoryExists()
  {
    // TODO: Use service instead of Directory.Exists:
    if (!Directory.Exists(_jsonStorageConfig.StoragePath))
    {
      Directory.CreateDirectory(_jsonStorageConfig.StoragePath);
    }
  }
}
