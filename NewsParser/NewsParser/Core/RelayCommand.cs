using System;
using System.Windows.Input;

namespace NewsParser.Core
{
    internal abstract class RelayCommand : ICommand
    {
        private Action<object>? _execute;

        private Func<object, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);
    }
}
