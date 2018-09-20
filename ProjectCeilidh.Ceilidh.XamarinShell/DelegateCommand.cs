using System;
using System.Windows.Input;

namespace ProjectCeilidh.Ceilidh.XamarinShell
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();

        public void OnCanExecuteChanged() => OnCanExecuteChanged(EventArgs.Empty);

        public void OnCanExecuteChanged(EventArgs e) => CanExecuteChanged?.Invoke(this, e);

        public event EventHandler CanExecuteChanged;
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke((T) Convert.ChangeType(parameter, typeof(T))) ?? true;

        public void Execute(object parameter) => _execute((T) Convert.ChangeType(parameter, typeof(T)));

        public void OnCanExecuteChanged() => OnCanExecuteChanged(EventArgs.Empty);

        public void OnCanExecuteChanged(EventArgs e) => CanExecuteChanged?.Invoke(this, e);

        public event EventHandler CanExecuteChanged;
    }
}
