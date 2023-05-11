namespace TapPlayer.Core.Services;

public interface IDateTimeService
{
  DateTimeOffset UtcNow { get; }
}
