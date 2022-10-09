using System.Windows.Input;

namespace Explorer.Shared.ViewModels;

public class DelegateCommand : ICommand
{
    private readonly Action<object> _open; //readonly - присвоить значение можно только в конструкторе этого класса
    public DelegateCommand(Action<object> open)
    {
        _open = open;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _open?.Invoke(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}