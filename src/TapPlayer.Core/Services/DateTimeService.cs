namespace TapPlayer.Core.Services;

public class DateTimeService : IDateTimeService
{
  public DateTimeOffset UtcNow { get => DateTimeOffset.UtcNow; }
}
