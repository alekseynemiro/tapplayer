using CommunityToolkit.Mvvm.Messaging;

namespace TapPlayer.Maui.Services;

public class ActiveProjectService : IActiveProjectService
{
  private Guid _projectId;

  public Guid ProjectId => _projectId;

  public bool HasProject => ProjectId != Guid.Empty;

  public void Set(Guid projectId)
  {
    _projectId = projectId;

    WeakReferenceMessenger.Default.Send<IActiveProjectService>(this);
  }

  public void Reset()
  {
    _projectId = Guid.Empty;

    WeakReferenceMessenger.Default.Send<IActiveProjectService>(this);
  }
}
