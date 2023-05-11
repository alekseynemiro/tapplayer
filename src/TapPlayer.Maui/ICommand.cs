using System.Windows.Input;

namespace TapPlayer.Maui;

public interface ICommand<T> : ICommand
{
  void Execute(T parameter);

  bool CanExecute(T parameter);
}
