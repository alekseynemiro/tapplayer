using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui.Components;

public partial class ProjectEdit : ContentView
{
  private readonly ILogger _logger;
  private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

  private CancellationTokenSource _cancellationTokenSource;
  private CancellationToken _cancellationToken;

  private int _gridSizeSelectedIndexChangedQueue = 0;
  private bool _isRendered = false;

  public IProjectEditViewModel Model => (IProjectEditViewModel)BindingContext;

  public ProjectEdit()
  {
    _logger = MauiProgram.ServiceProvider.GetRequiredService<ILogger<ProjectEdit>>();

    InitializeComponent();
  }

  protected void ProjectEdit_LayoutChanged(object sender, EventArgs e)
  {
    _logger.LogDebug(nameof(ProjectEdit_LayoutChanged));

    _isRendered = true;

    if (Model != null && Model.IsLoaded)
    {
      GridSize_SelectedIndexChanged(GridSize, default);
    }
  }

  protected void ProjectEdit_BindingContextChanged(object sender, EventArgs e)
  {
    _logger.LogDebug(nameof(ProjectEdit_BindingContextChanged) + ": {Model}.", Model == null ? "<NULL>" : "New model");

    if (Model != null && Model.IsLoaded && _isRendered)
    {
      GridSize_SelectedIndexChanged(GridSize, default);
    }
  }

  protected void ProjectEdit_Unloaded(object sender, EventArgs e)
  {
    _logger.LogDebug(nameof(ProjectEdit_Unloaded));
    Dispose();
  }

  protected void GridSize_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (Model?.IsLoaded != true)
    {
      _logger.LogDebug(
        nameof(GridSize_SelectedIndexChanged) + ": SKIP, project loaded = {IsLoaded}.",
        Model?.IsLoaded
      );
      return;
    }

    _logger.LogDebug(
      nameof(GridSize_SelectedIndexChanged) + ": IsCancellationRequested = {IsCancellationRequested}, Queue = {Queue}, SemaphoreCurrentCount = {SemaphoreCurrentCount}",
      _cancellationToken.IsCancellationRequested,
      _gridSizeSelectedIndexChangedQueue,
      _semaphore.CurrentCount
    );

    if (_cancellationToken != CancellationToken.None)
    {
      _cancellationTokenSource.Cancel();
    }

    var tokenSource = new CancellationTokenSource();
    var token = tokenSource.Token;

    _cancellationTokenSource = tokenSource;
    _cancellationToken = token;

    Task.Run(
      async () =>
      {
        token.ThrowIfCancellationRequested();

        Interlocked.Increment(ref _gridSizeSelectedIndexChangedQueue);

        _semaphore.Wait(TimeSpan.FromSeconds(30));

        token.ThrowIfCancellationRequested();

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
            token.ThrowIfCancellationRequested();
            await Task.Delay(250);
          }

          if (_gridSizeSelectedIndexChangedQueue <= 1)
          {
            token.ThrowIfCancellationRequested();

            var stopwatch = new Stopwatch();
            var tilesGrid = new Grid
            {
              HorizontalOptions = LayoutOptions.Fill,
              VerticalOptions = LayoutOptions.Fill,
              RowSpacing = 2,
              ColumnSpacing = 2,
            };

            stopwatch.Start();
            tilesGrid.Create(gridSize, CreateTill);
            stopwatch.Stop();

            _logger.LogInformation("Tiles were created in {Elapsed}.", stopwatch.Elapsed);

            token.ThrowIfCancellationRequested();

            Dispatcher.Dispatch(() =>
            {
              TilesGridContainer.Content = tilesGrid;
            });
          }
        }
        catch (OperationCanceledException)
        {
          _logger.LogInformation("Tiles generation has been cancelled. Queue size: {QueueSize}.", _gridSizeSelectedIndexChangedQueue);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "An error occurred while initializing tiles.");
        }
        finally
        {
          Interlocked.Decrement(ref _gridSizeSelectedIndexChangedQueue);

          if (Model != null)
          {
            Model.CanSetGridSize = _gridSizeSelectedIndexChangedQueue == 0;
          }

          _semaphore.Release();

          _logger.LogDebug(
            nameof(GridSize_SelectedIndexChanged) + " Finished: IsCancellationRequested = {IsCancellationRequested}, Queue = {Queue}, SemaphoreCurrentCount = {SemaphoreCurrentCount}",
            _cancellationToken.IsCancellationRequested,
            _gridSizeSelectedIndexChangedQueue,
            _semaphore.CurrentCount
          );
        }
      },
      token
    );
  }

  private IView CreateTill(GridCreateEventArgs e)
  {
    if (_cancellationToken != CancellationToken.None)
    {
      _cancellationToken.ThrowIfCancellationRequested();
    }

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
    Model?.CancelCommand?.Execute(null);
  }

  protected void MainScrollView_SizeChanged(object sender, EventArgs e)
  {
    _logger.LogDebug(nameof(MainScrollView_SizeChanged));

    if (Model == null)
    {
      return;
    }

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

  public void Init()
  {
    _logger.LogDebug(nameof(Init));

    BindingContextChanged += ProjectEdit_BindingContextChanged;
    LayoutChanged += ProjectEdit_LayoutChanged;
    Unloaded += ProjectEdit_Unloaded;

    GridSize.SelectedIndexChanged += GridSize_SelectedIndexChanged;
    MainScrollView.SizeChanged += MainScrollView_SizeChanged;

    // TODO: I don't like this solution. Too confusing.
    // We need to find a way to update the Grid when the model changes, through binding.
    // https://github.com/alekseynemiro/TapPlayer/issues/21
    WeakReferenceMessenger.Default.Register<IProjectEditViewModel>(
      this,
      (r, m) =>
      {
        _logger.LogDebug(
          "Project {ProjectName} ({ProjectId}): Loaded = {IsLoaded}.",
          m.ProjectName ?? "<NULL>",
          m.ProjectId,
          m.IsLoaded
        );

        if (m.IsLoaded)
        {
          GridSize_SelectedIndexChanged(GridSize, default);
        }
      }
    );
  }

  public void Dispose()
  {
    _logger.LogDebug(nameof(Dispose));

    BindingContextChanged -= ProjectEdit_BindingContextChanged;
    LayoutChanged -= ProjectEdit_LayoutChanged;
    Unloaded -= ProjectEdit_Unloaded;

    GridSize.SelectedIndexChanged -= GridSize_SelectedIndexChanged;
    MainScrollView.SizeChanged -= MainScrollView_SizeChanged;

    if (_cancellationToken != CancellationToken.None)
    {
      _cancellationTokenSource.Cancel();
    }

    WeakReferenceMessenger.Default.Unregister<IProjectEditViewModel>(this);
  }
}
