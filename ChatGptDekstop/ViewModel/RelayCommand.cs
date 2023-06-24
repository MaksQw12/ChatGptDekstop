using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatGptDekstop.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Func<Task> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke() ?? true;
        }

        public async void Execute(object parameter)
        {
            await execute();
        }


    }
}
