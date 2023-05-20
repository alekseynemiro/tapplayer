using Plugin.Maui.Audio;
using TapPlayer.Maui.Services;

namespace TapPlayer.Maui.ViewModels;

public class MediaPlayerViewModel : IMediaPlayerViewModel
{
  private readonly IDialogService _dialogService;
  private readonly IAudioPlayer _player;
  private readonly Stream _fileStream;

  public bool IsPlaying => _player.IsPlaying;

  public bool Loop { get; set; }

  public MediaPlayerViewModel(
    IAudioManager audioManager,
    IDialogService dialogService,
    string filePath
  )
  {
    _dialogService = dialogService;
    _fileStream = TryToOpenFileStream(filePath); // TODO: looks bad, need to find another solution
    _player = audioManager.CreatePlayer(_fileStream);
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
    _player.Dispose();

    if (_fileStream != null)
    {
      _fileStream.Close();
      _fileStream.Dispose();
    }
  }

  private Stream TryToOpenFileStream(string filePath)
  {
    try
    {
      return File.OpenRead(filePath);
    }
    catch (Exception ex)
    {
      _dialogService.Error($"Failed to open file \"{filePath}\": {ex.Message}");
      return null;
    }
  }
}
