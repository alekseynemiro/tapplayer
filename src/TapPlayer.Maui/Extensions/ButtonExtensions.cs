using Microsoft.Maui.Graphics.Skia;
using Font = Microsoft.Maui.Graphics.Font;

namespace TapPlayer.Maui.Extensions;

public static class ButtonExtensions
{
  public static void FitTextToSize(this Button button)
  {
    if (string.IsNullOrWhiteSpace(button.Text))
    {
      return;
    }

    double buttonContentWidth = button.Width - button.Padding.HorizontalThickness;
    double buttonContentHeight = button.Height - button.Padding.VerticalThickness;

    if (Math.Min(buttonContentWidth, buttonContentHeight) <= 0)
    {
      return;
    }

    double width = buttonContentWidth - (buttonContentWidth * 0.1);
    double height = buttonContentHeight - (buttonContentHeight * 0.3);

    double ratio = Math.Min(width, height);

    if (ratio <= 0)
    {
      return;
    }

    using var bmp = new SkiaBitmapExportContext((int)width, (int)height, (float)DeviceDisplay.MainDisplayInfo.Density);
    var font = new Font(button.FontFamily);
    var canvas = bmp.Canvas;

    float step = 2.0F;
    float fontSize = (float)ratio;

    var size = canvas.GetStringSize(
      button.Text,
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
        button.Text,
        font,
        fontSize,
        HorizontalAlignment.Left,
        VerticalAlignment.Top
      );
    }

    button.FontSize = fontSize;
  }
}
