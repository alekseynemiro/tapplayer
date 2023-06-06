using CommunityToolkit.Mvvm.Messaging;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Components;

public partial class ProjectEdit : ContentView
{
  private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

  private int _gridSizeSelectedIndexChangedQueue = 0;
  private bool _isRendered = false;

  public IProjectEditViewModel Model => (IProjectEditViewModel)BindingContext;

  public ProjectEdit()
  {
    InitializeComponent();

    BindingContextChanged += ProjectEdit_BindingContextChanged;
    LayoutChanged += ProjectEdit_LayoutChanged;
    Unloaded += ProjectEdit_Unloaded;

    GridSize.SelectedIndexChanged += GridSize_SelectedIndexChanged;

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

  protected void ProjectEdit_LayoutChanged(object sender, EventArgs e)
  {
    _isRendered = true;
  }

  protected void ProjectEdit_BindingContextChanged(object sender, EventArgs e)
  {
    if (Model != null && Model.IsLoaded && _isRendered)
    {
      GridSize_SelectedIndexChanged(GridSize, default);
    }
  }

  protected void ProjectEdit_Unloaded(object sender, EventArgs e)
  {
    // TODO: Cancel tasks
  }

  protected void GridSize_SelectedIndexChanged(object sender, EventArgs e)
  {
    Interlocked.Increment(ref _gridSizeSelectedIndexChangedQueue);

    Task.Run(
      async () =>
      {
        _semaphore.Wait();

        if (Model == null)
        {
          _semaphore.Release();
          return;
        }

        Model.CanSetGridSize = false;

        var gridSize = (GridSize)GridSize.SelectedItem;

        Dispatcher.Dispatch(() =>
        {
          TilesGridContainer.Content = new ActivityIndicator
          {
            IsRunning = true,
            Scale = 0.25,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
          };
        });

        try
        {
          while (!_isRendered || !Model.IsLoaded)
          {
            await Task.Delay(250);
          }

          if (_gridSizeSelectedIndexChangedQueue == 1)
          {
            var tilesGrid = new Grid
            {
              HorizontalOptions = LayoutOptions.Fill,
              VerticalOptions = LayoutOptions.Fill,
              RowSpacing = 2,
              ColumnSpacing = 2,
            };

            tilesGrid.Create(gridSize, CreateTill);

            Dispatcher.Dispatch(() =>
            {
              TilesGridContainer.Content = tilesGrid;
            });
          }
        }
        finally
        {
          Interlocked.Decrement(ref _gridSizeSelectedIndexChangedQueue);
          Model.CanSetGridSize = _gridSizeSelectedIndexChangedQueue == 0;
          _semaphore.Release();
        }
      }
    );
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

      TilesVerticalStackLayout.DispatchInvalidateMeasure();
    }
  }
}
