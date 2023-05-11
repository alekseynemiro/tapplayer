namespace TapPlayer.Data;

public class JsonStorageConfig : IJsonStorageConfig
{
  // TODO: User services intead of Path and AppDomain

  public string StoragePath { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "projects"); }

  public string ProjectListFileName { get => "projectList.json"; }

  public string ProjectFileExtension { get => ".json"; }
}
