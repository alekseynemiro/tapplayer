using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;

namespace TapPlayer.Maui.ViewModels;

public class MediaPlayerViewModel : IMediaPlayerViewModel
{
  private readonly MediaElement _player;

  public bool IsPlaying => _player.CurrentState == MediaElementState.Playing;

  public bool Loop { get; set; }

  public MediaPlayerViewModel(MediaElement mediaElement)
  {
    _player = mediaElement;
    _player.MediaEnded += MediaEnded;
  }

  public void Pause()
  {
    _player.Pause();
  }

  public void Play()
  {
    _player.Play();
  }

  public void Stop()
  {
    _player.Stop();
  }

  public void Dispose()
  {
    _player.Handler?.DisconnectHandler();
  }

  private void MediaEnded(object sender, EventArgs e)
  {
    if (Loop)
    {
      Stop();
      Play();
    }
  }
}
