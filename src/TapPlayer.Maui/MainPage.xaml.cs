using TapPlayer.Maui.Components;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class MainPage : ContentPage
{
  private readonly IActiveProjectService _activeProjectService;
  private IMainPageViewModel Model => (IMainPageViewModel)BindingContext;

  public MainPage(
    IActiveProjectService activeProjectService,
    IMainPageViewModel model
  )
  {
    _activeProjectService = activeProjectService;
    BindingContext = model;

    InitializeComponent();
    Init();
  }

  private void Init()
  {
    Task.Run(async () =>
    {
      Dispatcher.Dispatch(TilesGridContainer.Children.Clear);

      await Model.InitCommand.ExecuteAsync();

      if (_activeProjectService.HasProject)
      {
        await Model.LoadCommand.ExecuteAsync(_activeProjectService.ProjectId);

        var tilesGrid = new Grid
        {
          VerticalOptions = LayoutOptions.Fill,
          HorizontalOptions = LayoutOptions.Fill,
          RowSpacing = 2,
          ColumnSpacing = 2,
        };

        tilesGrid.Create(
          Model.GridSize,
          CreateTill
        );

        Dispatcher.Dispatch(() =>
        {
          TilesGridContainer.Children.Add(tilesGrid);
          TilesGridContainer.DispatchInvalidateMeasure();
          Model.HideActivityIndicator();
        });
      }
      else
      {
        Model.HideActivityIndicator();
      }
    });
  }

  private IView CreateTill(GridCreateEventArgs e)
  {
    var tile = Model.Tiles.ElementAt(e.Index);
    var tileView = new TileView
    {
      BindingContext = tile,
      HorizontalOptions = LayoutOptions.Fill,
      VerticalOptions = LayoutOptions.Fill,
    };

    return tileView;
  }
}
