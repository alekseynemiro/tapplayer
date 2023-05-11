namespace TapPlayer.Data;

public interface IJsonStorageConfig
{
  string StoragePath { get; }
  string ProjectListFileName { get; }
  string ProjectFileExtension { get; }
}
