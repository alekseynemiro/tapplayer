using Microsoft.Extensions.Logging;
using System.ComponentModel;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class TileEditPage : ContentPage
{
  private readonly ILogger _logger;
  private readonly INavigationService _navigationService;

  public ITileEditPageViewModel Model => (ITileEditPageViewModel)BindingContext;

  public TileEditPage(
    ILogger<TileEditPage> logger,
    INavigationService navigationService,
    ITileEditPageViewModel model
  )
  {
    _logger = logger;
    _navigationService = navigationService;

    InitializeComponent();

    BindingContext = model;
  }

  private void SelectFile_Clicked(object sender, EventArgs e)
  {
    _logger.LogDebug(nameof(SelectFile_Clicked));

    Dispatcher.Dispatch(async () =>
    {
      var file = await FilePicker.Default.PickAsync(new PickOptions
      {
        PickerTitle = "Please select an audio file",
        FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
          {
              { DevicePlatform.Android, new[] { "audio/*" } },
              { DevicePlatform.WinUI, new[] { ".mp3", ".wav" } },
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
    _logger.LogDebug(nameof(SelectColor_Clicked));

    Dispatcher.Dispatch(async () =>
    {
      var selectColorPage = new SelectColorPage();

      selectColorPage.Selected += SelectColorPage_Selected;

      await _navigationService.PushModalAsync(selectColorPage);
    });
  }

  private void SelectColorPage_Selected(object sender, SelectColorEventArgs e)
  {
    _logger.LogDebug(nameof(SelectColorPage_Selected));

    Model.Color = e.Color;
  }

  private void IsBackground_Tapped(object sender, TappedEventArgs e)
  {
    _logger.LogDebug(nameof(IsBackground_Tapped));

    Model.IsBackground = !Model.IsBackground;
  }

  private void MaximumWidthRequest_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName.Equals(nameof(MaximumWidthRequest)))
    {
      ((IView)sender).DispatchInvalidateMeasure();
    }
  }
}
