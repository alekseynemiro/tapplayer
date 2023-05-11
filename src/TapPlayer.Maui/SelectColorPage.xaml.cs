using System.Collections.ObjectModel;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.Extensions;

// TODO: ViewModel

namespace TapPlayer.Maui;

public partial class SelectColorPage : ContentPage
{
  private readonly ObservableCollection<ColorPalette> _colors = new ObservableCollection<ColorPalette>(Enum.GetValues<ColorPalette>());

  private double _cellWidth;

  private double _cellHeight;

  public delegate void SelectedHandler(object sender, SelectColorEventArgs e);

  public event SelectedHandler Selected;

  public ObservableCollection<ColorPalette> Colors => _colors;

  public double CellWidth
  {
    get
    {
      return _cellWidth;
    }
    set
    {
      _cellWidth = value;
      OnPropertyChanged();
    }
  }

  public double CellHeight
  {
    get
    {
      return _cellHeight;
    }
    set
    {
      _cellHeight = value;
      OnPropertyChanged();
    }
  }

  public SelectColorPage()
  {
    InitializeComponent();
  }

  private void Color_Clicked(object sender, EventArgs e)
  {
    Dispatcher.Dispatch(async () =>
    {
      var button = (Button)sender;

      Selected(this, new SelectColorEventArgs((ColorPalette)button.CommandParameter));

      await Navigation.PopModalAsync();
    });
  }

  private void Cancel_Clicked(object sender, EventArgs e)
  {
    Dispatcher.Dispatch(() => Navigation.PopModalAsync());
  }

  private void ColorList_SizeChanged(object sender, EventArgs e)
  {
    var layout = (GridItemsLayout)ColorList.ItemsLayout;

    double width = ColorList.Width - (layout.Span * layout.HorizontalItemSpacing);
    double height = ColorList.Height - (layout.Span * layout.VerticalItemSpacing);

    if (width > 0 && height > 0)
    {
      CellWidth = width / layout.Span;
      CellHeight = height / layout.Span;

      ColorList.DispatchInvalidateMeasure();
    }
  }
}
