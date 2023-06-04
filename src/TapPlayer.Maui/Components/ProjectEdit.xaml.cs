using CommunityToolkit.Mvvm.Messaging;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Components;

public partial class ProjectEdit : ContentView
{
  public IProjectEditViewModel Model => (IProjectEditViewModel)BindingContext;

  public ProjectEdit()
  {
    InitializeComponent();

    // TODO: I don't like this solution. Too confusing.
    // We need to find a way to update the Grid when the model changes, through binding.
    // https://github.com/alekseynemiro/TapPlayer/issues/21
    WeakReferenceMessenger.Default.Register<IProjectEditViewModel>(
      this,
      (r, m) =>
      {
        if (m.IsLoaded)
        {
          GridSize_SelectedIndexChanged(GridSize, default);
        }
      }
    );
  }

  protected void ProjectEdit_BindingContextChanged(object sender, EventArgs e)
  {
    GridSize_SelectedIndexChanged(GridSize, default);
  }

  protected void GridSize_SelectedIndexChanged(object sender, EventArgs e)
  {
    var gridSize = (GridSize)GridSize.SelectedItem;

    TilesGrid.Create(gridSize, CreateTill);
    TilesGrid.DispatchInvalidateMeasure();
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

  protected void Cancel_Clicked(object sender, EventArgs e)
  {
    Model.CancelCommand.Execute(null);
  }

  protected void MainScrollView_SizeChanged(object sender, EventArgs e)
  {
    var scrollView = (ScrollView)sender;

    double width = scrollView.Width;
    double height = scrollView.Height;

    if (width > 0 && height > 0)
    {
      double min = Math.Min(
        width - scrollView.Padding.HorizontalThickness,
        height - scrollView.Padding.VerticalThickness
      );

      Model.TilesGridWidth = Model.TilesGridHeight = min;

      scrollView.DispatchInvalidateMeasure();
    }
  }

  protected void ContentPage_Refreshing(object sender, EventArgs e)
  {
    if (Model.IsLoaded)
    {
      Model.ShowRefreshing = false;
      // TODO: Reload?
    }
  }
}
