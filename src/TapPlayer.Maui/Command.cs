using System.Windows.Input;

namespace TapPlayer.Maui;

public class Command<T> : ICommand<T>
{
  public event EventHandler CanExecuteChanged;

  private bool _isExecuting;
  private readonly Action<T> _execute;
  private readonly Func<T, bool> _canExecute;

  public Command(Action<T> execute, Func<T, bool> canExecute = null)
  {
    _execute = execute;

    if (canExecute == null)
    {
      _canExecute = (T parameter) => true;
    }
    else
    {
      _canExecute = canExecute;
    }
  }

  public bool CanExecute(T parameter)
  {
    return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
  }

  public void Execute(T parameter)
  {
    if (CanExecute(parameter))
    {
      try
      {
        _isExecuting = true;
        _execute(parameter);
      }
      finally
      {
        _isExecuting = false;
      }
    }

    RaiseCanExecuteChanged();
  }

  public void RaiseCanExecuteChanged()
  {
    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
  }

  bool ICommand.CanExecute(object parameter)
  {
    return CanExecute((T)parameter);
  }

  void ICommand.Execute(object parameter)
  {
    Execute((T)parameter);
  }
}
