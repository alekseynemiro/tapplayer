using Microsoft.Maui.Graphics.Skia;
using TapPlayer.Data.Enums;
using TapPlayer.Maui.ViewModels;
using Font = Microsoft.Maui.Graphics.Font;

namespace TapPlayer.Maui.Components;

public partial class TileView : ContentView
{
  public ITileViewModel Model => (ITileViewModel)BindingContext;

  public TileView()
  {
    InitializeComponent();

    BindingContextChanged += TileView_BindingContextChanged;
    SizeChanged += TileView_SizeChanged;
  }

  protected void TileView_BindingContextChanged(object sender, EventArgs e)
  {
    if (Model == null)
    {
      return;
    }

    Model.Player?.Dispose();

    Model.Player = new MediaPlayerViewModel(MediaPlayer)
    {
      Loop = Model.PlayType == PlayType.Loop,
    };
  }

  protected void TileView_SizeChanged(object sender, EventArgs e)
  {
    if (string.IsNullOrWhiteSpace(TileName.Text))
    {
      return;
    }

    double contentWidth = Width - Padding.HorizontalThickness - TileName.Padding.HorizontalThickness;
    double contentHeight = Height - Padding.VerticalThickness - TileViewConainer.RowDefinitions[0].Height.Value - TileName.Padding.VerticalThickness;

    if (Math.Min(contentWidth, contentHeight) <= 0)
    {
      return;
    }

    double width = contentWidth - (contentWidth * 0.3);
    double height = contentHeight - (contentHeight * 0.3);

    double ratio = Math.Min(width, height);

    if (ratio <= 0)
    {
      return;
    }

    using var bmp = new SkiaBitmapExportContext((int)width, (int)height, (float)DeviceDisplay.MainDisplayInfo.Density);
    var font = new Font(TileName.FontFamily);
    var canvas = bmp.Canvas;

    if (canvas == null)
    {
      return;
    }

    float step = 2.0F;
    float fontSize = (float)ratio;

    var size = canvas.GetStringSize(
      TileName.Text,
      font,
      fontSize,
      HorizontalAlignment.Left,
      VerticalAlignment.Top
    );

    while (size.Width > width || size.Height > height)
    {
      fontSize -= step;

      if (fontSize < 8)
      {
        break;
      }

      size = canvas.GetStringSize(
        TileName.Text,
        font,
        fontSize,
        HorizontalAlignment.Left,
        VerticalAlignment.Top
      );
    }

    TileName.FontSize = fontSize;
  }
}
