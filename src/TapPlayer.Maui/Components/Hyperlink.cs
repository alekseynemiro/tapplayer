namespace TapPlayer.Maui.Components;

public class Hyperlink : Label
{
  public static readonly BindableProperty UrlProperty = BindableProperty.Create(nameof(Url), typeof(string), typeof(Hyperlink), null);

  public string Url
  {
    get
    {
      return (string)GetValue(UrlProperty);
    }
    set
    {
      SetValue(UrlProperty, value);
    }
  }

  public Hyperlink()
  {
    StyleClass = new List<string> { "Link" };

    GestureRecognizers.Add(new TapGestureRecognizer
    {
      Command = new Command(async () => await Launcher.OpenAsync(Url))
    });
  }
}
