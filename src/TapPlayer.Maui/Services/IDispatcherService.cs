﻿namespace TapPlayer.Maui.Services;

public interface IDispatcherService
{
  void Dispatch(Action action);

  Task DispatchAsync(Action action);
}
