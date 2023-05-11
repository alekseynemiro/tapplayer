using TapPlayer.Maui.ViewModels;

namespace TapPlayer.Maui;

public partial class ProjectListPage : ContentPage
{
  public IProjectListPageViewModel Model => (IProjectListPageViewModel)BindingContext;

  public ProjectListPage(IProjectListPageViewModel model)
  {
    BindingContext = model;

    InitializeComponent();

    Dispatcher.Dispatch(async () => await model.LoadCommand.ExecuteAsync());
  }

  protected void DataGrid_Refreshing(object sender, EventArgs e)
  {
    if (Model.IsLoaded)
    {
      Model.ShowRefreshing = false;
      // TODO: Reload?
    }
  }
}
