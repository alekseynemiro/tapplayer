using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TapPlayer.Maui.Components;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class MainPage : ContentPage, IDisposable
{
  private readonly ILogger _logger;
  private readonly IActiveProjectService _activeProjectService;
  private readonly CancellationTokenSource _initTaskCancellationTokenSource = new CancellationTokenSource();
  private CancellationToken _initTaskCancellationToken;

  private IMainPageViewModel Model => (IMainPageViewModel)BindingContext;

  private bool _disposed = false;

  public MainPage(
    ILogger<MainPage> logger,
    IActiveProjectService activeProjectService,
    IMainPageViewModel model
  )
  {
    _logger = logger;
    _activeProjectService = activeProjectService;
    BindingContext = model;

    InitializeComponent();
    Init();
  }

  private void Init()
  {
    _initTaskCancellationToken = _initTaskCancellationTokenSource.Token;

    Task.Run(
      async () =>
      {
        try
        {
          _initTaskCancellationToken.ThrowIfCancellationRequested();

          Dispatcher.Dispatch(TilesGridContainer.Children.Clear);

          await Model.InitCommand.ExecuteAsync();

          _initTaskCancellationToken.ThrowIfCancellationRequested();

          if (_activeProjectService.HasProject)
          {
            await Model.LoadCommand.ExecuteAsync(_activeProjectService.ProjectId);

            _initTaskCancellationToken.ThrowIfCancellationRequested();

            var stopwatch = new Stopwatch();
            var tilesGrid = new Grid
            {
              VerticalOptions = LayoutOptions.Fill,
              HorizontalOptions = LayoutOptions.Fill,
              RowSpacing = 2,
              ColumnSpacing = 2,
            };

            stopwatch.Start();

            tilesGrid.Create(
              Model.GridSize,
              CreateTill
            );

            stopwatch.Stop();

            _logger.LogInformation("Tiles were created in {Elapsed}.", stopwatch.Elapsed);

            _initTaskCancellationToken.ThrowIfCancellationRequested();

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
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Error while initializing project.");
        }
      },
      _initTaskCancellationToken
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

  public void Dispose()
  {
    if (_disposed)
    {
      return;
    }

    _initTaskCancellationTokenSource.Cancel();

    _disposed = true;
  }
}
