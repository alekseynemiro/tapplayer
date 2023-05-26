namespace TapPlayer.Maui.ViewModels;

public interface IMediaPlayerViewModel : IDisposable
{
  bool IsFailed { get; }
  bool IsPlaying { get; }
  bool Loop { get; set; }

  void Play();

  void Pause();

  void Stop();
}
