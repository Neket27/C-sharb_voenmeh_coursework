using System;
using System.Windows.Input;

namespace C_sharb_voenmeh_coursework.Command
{
    public class DelegateCommand : ICommand
    {
        #region Variebles

        private readonly Action<object> _execute; //readonly - присвоить значение можно только в конструкторе этого класса
        private readonly Predicate<object> _canExecute;

        #endregion

        #region Constructors
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Functions

        public bool CanExecute(object? parameter)
        {
            if (_canExecute != null)
                return _canExecute.Invoke(parameter);
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter);
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}