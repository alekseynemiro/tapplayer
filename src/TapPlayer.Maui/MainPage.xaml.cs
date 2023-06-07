using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TapPlayer.Maui.Components;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class MainPage : ContentPage
{
  private readonly ILogger _logger;
  private readonly IActiveProjectService _activeProjectService;

  private CancellationTokenSource _initTaskCancellationTokenSource;
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

    _logger.LogDebug("Instance created.");

    BindingContext = model;

    InitializeComponent();
  }

  private void Init()
  {
    _logger.LogDebug(nameof(Init));

    if (_initTaskCancellationToken != CancellationToken.None)
    {
      _initTaskCancellationTokenSource.Cancel();
    }

    _initTaskCancellationTokenSource = new CancellationTokenSource();

    var token = _initTaskCancellationTokenSource.Token;

    _initTaskCancellationToken = token;

    Task.Run(
      async () =>
      {
        try
        {
          token.ThrowIfCancellationRequested();

          Dispatcher.Dispatch(TilesGridContainer.Children.Clear);

          await Model.InitCommand.ExecuteAsync();

          token.ThrowIfCancellationRequested();

          if (_activeProjectService.HasProject)
          {
            await Model.LoadCommand.ExecuteAsync(_activeProjectService.ProjectId);

            token.ThrowIfCancellationRequested();

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

            token.ThrowIfCancellationRequested();

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
        catch (OperationCanceledException ex)
        {
          _logger.LogDebug(ex, "The task was canceled programmatically.");
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Error while initializing project.");
        }
      },
      token
    );
  }

  private IView CreateTill(GridCreateEventArgs e)
  {
    if (_initTaskCancellationToken != CancellationToken.None)
    {
      _initTaskCancellationToken.ThrowIfCancellationRequested();
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

  protected override void OnAppearing()
  {
    _logger.LogDebug(nameof(OnAppearing));
    base.OnAppearing();
    Init();
  }

  protected override void OnDisappearing()
  {
    _logger.LogDebug(nameof(OnDisappearing));
    base.OnDisappearing();
    _initTaskCancellationTokenSource.Cancel();
  }
}
