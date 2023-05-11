using MauiRefreshView = Microsoft.Maui.Controls.RefreshView;

namespace TapPlayer.Maui.Components;

public sealed class RefreshView : MauiRefreshView
{
  protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
  {
#if ANDROID
    return new SizeRequest(new Size(widthConstraint, heightConstraint));
#else
    return base.OnMeasure(widthConstraint, heightConstraint);
#endif
  }
}
