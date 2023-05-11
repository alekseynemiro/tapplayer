using Microsoft.Maui.Graphics.Skia;
using System.Text.RegularExpressions;
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

    // the text is sometimes cut off, we need to figure it out, but for now it's just a random numbers
    double kw = 4.2;
    double kh = 42;

    double width = button.Width - button.Padding.HorizontalThickness - kw;
    double height = button.Height - button.Padding.VerticalThickness - kh;
    double ratio = Math.Min(width, height);

    if (ratio <= 0)
    {
      return;
    }

    using var bmp = new SkiaBitmapExportContext((int)width, (int)height, 1.0f);
    var font = new Font(button.FontFamily);
    var canvas = bmp.Canvas;

    float step = 2.0F;
    float fontSize = (float)ratio;

    var size = canvas.GetStringSize(
      button.Text,
      font,
      fontSize,
      HorizontalAlignment.Center,
      VerticalAlignment.Center
    );

    while (size.Width > width || size.Height > height)
    {
      fontSize -= step;

      if (fontSize < 8)
      {
        break;
      }

      size = canvas.GetStringSize(button.Text, font, fontSize);
    }

    button.FontSize = fontSize;
  }
}
