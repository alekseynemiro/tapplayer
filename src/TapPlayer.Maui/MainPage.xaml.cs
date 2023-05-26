using TapPlayer.Maui.Components;
using TapPlayer.Maui.Extensions;
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
    var tileView = new TileView
    {
      BindingContext = tile,
      HorizontalOptions = LayoutOptions.Fill,
      VerticalOptions = LayoutOptions.Fill,
    };

    return tileView;
  }
}
