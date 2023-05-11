namespace TapPlayer.Maui.ViewModels;

public class FileViewModel : ViewModelBase
{
  private string _fullPath;

  public string FullPath
  {
    get
    {
      return _fullPath;
    }
    set
    {
      _fullPath = value;
      OnProprtyChanged();
    }
  }

  public string Name
  {
    get
    {
      return string.IsNullOrWhiteSpace(_fullPath)
        ? null
        // TODO: Use service instead of Path.GetFileName
        : Path.GetFileName(_fullPath);
    }
  }

  public FileViewModel(string fullPath = null)
  {
    _fullPath = fullPath;
  }
}
