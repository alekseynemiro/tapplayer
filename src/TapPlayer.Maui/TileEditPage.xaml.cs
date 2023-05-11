using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class TileEditPage : ContentPage
{
  private readonly INavigationService _navigationService;

  public ITileEditPageViewModel Model => (ITileEditPageViewModel)BindingContext;

  public TileEditPage(
    INavigationService navigationService,
    ITileEditPageViewModel model
  )
  {
    _navigationService = navigationService;

    InitializeComponent();

    BindingContext = model;
  }

  private void SelectFile_Clicked(object sender, EventArgs e)
  {
    Dispatcher.Dispatch(async () =>
    {
      var file = await FilePicker.Default.PickAsync(new PickOptions
      {
        PickerTitle = "Please select an audio file",
        FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
          {
              { DevicePlatform.Android, new[] { "audio/mpeg", "audio/wav" } },
              { DevicePlatform.WinUI, new[] { ".mp3", ".wav" } },
              { DevicePlatform.macOS, new[] { "mp3", "wav" } },
          }
        ),
      });

      if (!string.IsNullOrWhiteSpace(file?.FullPath))
      {
        Model.File = new FileViewModel(file.FullPath);
      }
    });
  }

  private void SelectColor_Clicked(object sender, EventArgs e)
  {
    Dispatcher.Dispatch(async () =>
    {
      var selectColorPage = new SelectColorPage();

      selectColorPage.Selected += SelectColorPage_Selected;

      await _navigationService.PushModalAsync(selectColorPage);
    });
  }

  private void SelectColorPage_Selected(object sender, SelectColorEventArgs e)
  {
    Model.Color = e.Color;
  }

  private void IsBackground_Tapped(object sender, TappedEventArgs e)
  {
    Model.IsBackground = !Model.IsBackground;
  }
}
