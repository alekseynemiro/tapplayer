namespace TapPlayer.Maui.Services;

public interface IActiveProjectService
{
  Guid ProjectId { get; }

  bool HasProject { get; }

  void Set(Guid projectId);

  void Reset();
}
