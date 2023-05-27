using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;

namespace TapPlayer.Maui.ViewModels;

public class MediaPlayerViewModel : ViewModelBase, IMediaPlayerViewModel
{
  private readonly MediaElement _player;
  private bool _isFailed = false;
  private bool _isPlaying = false;
  private bool _loop = false;

  public bool IsFailed
  {
    get
    {
      return _isFailed;
    }
    private set
    {
      _isFailed = value;
      OnProprtyChanged();
    }
  }

  public bool IsPlaying
  {
    get
    {
      return _isPlaying;
    }
    private set
    {
      _isPlaying = value;
      OnProprtyChanged();
    }
  }

  public bool Loop
  {
    get
    {
      return _loop;
    }
    set
    {
      _loop = value;
      OnProprtyChanged();
    }
  }

  public MediaPlayerViewModel(MediaElement mediaElement)
  {
    _player = mediaElement;

    _player.MediaEnded += Player_MediaEnded;
    _player.StateChanged += Player_StateChanged;
  }

  public void Pause()
  {
    _player.Pause();
  }

  public void Play()
  {
    _player.Stop();
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

  private void Player_MediaEnded(object sender, EventArgs e)
  {
    if (Loop)
    {
      Application.Current.Dispatcher.Dispatch(Play);
    }
  }

  private void Player_StateChanged(object sender, MediaStateChangedEventArgs e)
  {
    IsPlaying = e.NewState == MediaElementState.Playing;
    IsFailed = e.NewState == MediaElementState.Failed;
  }
}
