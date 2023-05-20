using CommunityToolkit.Maui.Views;
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

        MainGrid.Create(
          model.GridSize,
          CreateTill
        );
      }
    });
  }

  private IView CreateTill(GridCreateEventArgs e)
  {
    var tile = Model.Tiles.ElementAt(e.Index);

    var container = new Grid
    {
      HorizontalOptions = LayoutOptions.Fill,
      VerticalOptions = LayoutOptions.Fill,
      Padding = Thickness.Zero,
      Margin = Thickness.Zero,
    };

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

    container.Add(button);

    if (!string.IsNullOrEmpty(tile.File?.FullPath))
    {
      var mediaElement = new MediaElement
      {
        Source = MediaSource.FromFile(tile.File.FullPath),
        IsVisible = false,
      };

      // TODO: Looks bad. We need to find a better solution.
      // Model should not know anything about view elements.
      tile.Player = new MediaPlayerViewModel(mediaElement)
      {
        Loop = tile.PlayType == Data.Enums.PlayType.Loop,
      };

      container.Add(mediaElement);
    }

    return container;
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
