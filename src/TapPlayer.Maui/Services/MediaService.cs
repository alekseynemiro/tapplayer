using Plugin.Maui.Audio;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Services;

// TODO: Factory?
public class MediaService : IMediaService
{
  private readonly IServiceProvider _serviceProvider;

  public MediaService(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public IMediaPlayerViewModel CreatePlayer(string path)
  {
    return new MediaPlayerViewModel(
      _serviceProvider.GetRequiredService<IAudioManager>(),
      _serviceProvider.GetRequiredService<IDialogService>(),
      path
    );
  }
}
