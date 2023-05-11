using System.Windows.Input;

namespace TapPlayer.Maui
{
  public class AsyncCommand : AsyncCommand<object>, IAsyncCommand
  {
    public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
    {
    }

    public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null) : base(execute, canExecute)
    {
    }

    public bool CanExecute()
    {
      return base.CanExecute(null);
    }

    public Task ExecuteAsync()
    {
      return ExecuteAsync(null);
    }
  }

  public class AsyncCommand<T> : IAsyncCommand<T>
  {
    public event EventHandler CanExecuteChanged;

    private bool _isExecuting;
    private readonly Func<T, Task> _execute;
    private readonly Func<T, bool> _canExecute;

    public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
    {
      _execute = execute;

      if (canExecute == null)
      {
        _canExecute = (T parameter) => true;
      }
      else
      {
        _canExecute = (T parameter) => canExecute(parameter);
      }
    }

    public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
    {
      _execute = (T parameter) => execute();

      if (canExecute == null)
      {
        _canExecute = (T parameter) => true;
      }
      else
      {
        _canExecute = (T parameter) => canExecute();
      }
    }

    public bool CanExecute(T parameter = default(T))
    {
      return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
    }

    public async Task ExecuteAsync(T parameter = default(T))
    {
      if (CanExecute(parameter))
      {
        try
        {
          _isExecuting = true;
          await _execute(parameter);
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

    async void ICommand.Execute(object parameter)
    {
      await ExecuteAsync((T)parameter);
    }
  }
}
