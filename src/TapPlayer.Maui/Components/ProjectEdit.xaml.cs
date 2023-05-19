using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui;
using System.ComponentModel;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Converters;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Resources.Strings;
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

  private Button CreateTill(GridCreateEventArgs e)
  {
    var tile = Model.Tiles.ElementAt(e.Index);
    var button = new Button
    {
      BindingContext = tile,
      CommandParameter = Model,
      HorizontalOptions = LayoutOptions.Fill,
      VerticalOptions = LayoutOptions.Fill,
      LineBreakMode = LineBreakMode.NoWrap,
    };

    button.SetBinding(
      Button.StyleProperty,
      nameof(TileViewModel.Color),
      converter: new ColorPaletteToButtonStyleConverter()
    );

    button.SetBinding(
      Button.TextProperty,
      nameof(TileViewModel.Name)
    );

    button.SetBinding(
      Button.CommandProperty,
      nameof(TileViewModel.EditCommand)
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
