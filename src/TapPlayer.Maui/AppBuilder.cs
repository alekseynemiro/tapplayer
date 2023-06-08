using CommunityToolkit.Maui;
using MetroLog.MicrosoftExtensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TapPlayer.Core.Services;
using TapPlayer.Core.Services.Projects;
using TapPlayer.Data;
using TapPlayer.Maui.Extensions;
using TapPlayer.Maui.Resources.Strings;
using TapPlayer.Maui.Services;
using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public static class AppBuilder
{
  public static MauiAppBuilder CreateBuilder()
  {
    var builder = MauiApp.CreateBuilder();

    builder
      .UseMauiApp<App>()
      .UseMauiCommunityToolkit()
      .UseMauiCommunityToolkitMediaElement()
      .ConfigureFonts(fonts =>
      {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
      });

    // logging
    builder.Logging
      .SetMinimumLevel(LogLevel.Trace)
      .AddTraceLogger(options =>
      {
        options.MinLevel = LogLevel.Trace;
        options.MaxLevel = LogLevel.Critical;
      })
      .AddConsoleLogger(options =>
      {
#if DEBUG
        options.MinLevel = LogLevel.Trace;
#else
        options.MinLevel = LogLevel.Information;
#endif
        options.MaxLevel = LogLevel.Critical;
      }) // will write to the Console Output (logcat for android)
      .AddStreamingFileLogger(options =>
      {
        options.RetainDays = 2;
        options.FolderPath = Path.Combine(FileSystem.CacheDirectory, "logs");
      });

    // localization
    builder.Services.AddLocalization();
    builder.Services.AddSingleton<IStringLocalizer<CommonStrings>, StringLocalizer<CommonStrings>>();

    // storage
    builder.Services.AddSingleton<IJsonStorage, JsonStorage>();
    builder.Services.AddSingleton<IJsonStorageConfig, JsonStorageConfig>();

    // common services
    builder.Services.AddSingleton(Preferences.Default);
    builder.Services.AddSingleton<IAppSettingsService, AppSettingsService>();
    builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
    builder.Services.AddSingleton<IProjectListService, ProjectListService>();
    builder.Services.AddSingleton<IProjectService, ProjectService>();

    // ui services
    builder.Services.AddSingleton<IActiveProjectService, ActiveProjectService>();
    builder.Services.AddSingleton<IAppInfo, Services.AppInfo>();
    builder.Services.AddSingleton<IDialogService, DialogService>();
    builder.Services.AddSingleton<IDispatcherService, DispatcherService>();
    builder.Services.AddSingleton<IKeyboardService, KeyboardService>();
    builder.Services.AddSingleton<INavigationService, NavigationService>();
    builder.Services.AddSingleton<ITapPlayerService, TapPlayerService>();

    // view models
    builder.Services.AddSingleton<IAppShellViewModel, AppShellViewModel>();
    builder.Services.AddTransient<IAboutPageViewModel, AboutPageViewModel>();
    builder.Services.AddTransient<IAppSettingsPageViewModel, AppSettingsPageViewModel>();
    builder.Services.AddTransient<IMainPageViewModel, MainPageViewModel>();
    builder.Services.AddTransient<IProjectEditViewModel, ProjectEditViewModel>();
    builder.Services.AddTransient<IProjectListItemViewModel, ProjectListItemViewModel>();
    builder.Services.AddTransient<IProjectListPageViewModel, ProjectListPageViewModel>();
    builder.Services.AddTransient<ITileEditPageViewModel, TileEditPageViewModel>();
    builder.Services.AddTransient<ITileViewModel, TileViewModel>();

    // shell
    builder.Services.AddTransient<AppShell>();

    // pages
    builder.Services.AddPage<AboutPage>();
    builder.Services.AddPage<AppSettingsPage>();
    builder.Services.AddPage<MainPage>();
    builder.Services.AddPage<ProjectEditPage>();
    builder.Services.AddPage<ProjectListPage>();
    builder.Services.AddPage<ProjectSettingsPage>();
    builder.Services.AddPage<TileEditPage>();

    return builder;
  }
}
