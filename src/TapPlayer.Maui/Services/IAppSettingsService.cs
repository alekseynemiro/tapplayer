namespace TapPlayer.Maui.Services;

public interface IAppSettingsService
{
  Guid LastProjectId { get; set; }
  int Language { get; set; }
}
