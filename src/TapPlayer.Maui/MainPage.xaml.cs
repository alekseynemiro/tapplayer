using System.ComponentModel;
using TapPlayer.Maui.Converters;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Resources.Strings;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class MainPage : ContentPage
{
  private IMainPageViewModel Model => (IMainPageViewModel)BindingContext;

  public MainPage(
    IActiveProjectService activeProjectService,
    IMainPageViewModel model
  )
  {
    BindingContext = model;

    InitializeComponent();

    Dispatcher.DispatchAsync(async () =>
    {
      await model.InitCommand.ExecuteAsync();

      if (activeProjectService.HasProject)
      {
        await model.LoadCommand.ExecuteAsync(activeProjectService.ProjectId);

        MainGrid.Create(model.GridSize, CreateTill);
      }
    });
  }

  private Button CreateTill(GridCreateEventArgs e)
  {
    var tile = Model.Tiles.ElementAt(e.Index);

    var button = new Button
    {
      BindingContext = tile,
      Command = tile.TapCommand,
      VerticalOptions = LayoutOptions.Fill,
      HorizontalOptions = LayoutOptions.Fill,
    };

    button.SetBinding(
      Button.TextProperty,
      nameof(TileViewModel.Name)
    );

    button.SetBinding(
      Button.StyleProperty,
      nameof(TileViewModel.Color),
      converter: new ColorPaletteToButtonStyleConverter()
    );

    button.PropertyChanged += Tile_PropertyChanged;

    SemanticProperties.SetDescription(
      button,
      string.Format(CommonStrings.TileX, tile.Index + 1)
    );

    return button;
  }

  private void Tile_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (
      e.PropertyName == nameof(Button.Text)
      || e.PropertyName == nameof(Button.Width)
      || e.PropertyName == nameof(Button.Height)
      || e.PropertyName == nameof(Button.Padding)
    )
    {
      var button = (Button)sender;

      button.FitTextToSize();
    }
  }
}
