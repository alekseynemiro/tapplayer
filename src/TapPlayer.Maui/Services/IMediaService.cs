using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Services;

public interface IMediaService
{
  IMediaPlayerViewModel CreatePlayer(string path);
}
