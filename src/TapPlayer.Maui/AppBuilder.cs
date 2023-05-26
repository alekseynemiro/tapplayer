using CommunityToolkit.Maui;
using Microsoft.Extensions.Localization;
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

    // localization
    builder.Services.AddLocalization();
    builder.Services.AddSingleton<IStringLocalizer<CommonStrings>, StringLocalizer<CommonStrings>>();

    // storage
    builder.Services.AddSingleton<IJsonStorage, JsonStorage>();
    builder.Services.AddSingleton<IJsonStorageConfig, JsonStorageConfig>();

    // common services
    builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
    builder.Services.AddSingleton<IProjectListService, ProjectListService>();
    builder.Services.AddSingleton<IProjectService, ProjectService>();

    // ui services
    builder.Services.AddSingleton<IActiveProjectService, ActiveProjectService>();
    builder.Services.AddSingleton<IAppInfo, Services.AppInfo>();
    builder.Services.AddSingleton<IDialogService, DialogService>();
    builder.Services.AddSingleton<IKeyboardService, KeyboardService>();
    builder.Services.AddSingleton<INavigationService, NavigationService>();
    builder.Services.AddSingleton<ITapPlayerService, TapPlayerService>();

    // view models
    builder.Services.AddSingleton<IAppShellViewModel, AppShellViewModel>();
    builder.Services.AddTransient<IAboutPageViewModel, AboutPageViewModel>();
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
    builder.Services.AddPage<MainPage>();
    builder.Services.AddPage<ProjectEditPage>();
    builder.Services.AddPage<ProjectListPage>();
    builder.Services.AddPage<ProjectSettingsPage>();
    builder.Services.AddPage<TileEditPage>();

    return builder;
  }
}
