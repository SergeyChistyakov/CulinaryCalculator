using System;
using System.ComponentModel;
using System.Windows.Input;

namespace CulinaryCalculator.Infrastructure
{
    public class PropertyDependentCommand: ICommand
    {
        private readonly Func<object, bool> m_CanExecuteFunc;
        private readonly Action<object> m_ExecuteAction;

        public PropertyDependentCommand(INotifyPropertyChanged hostViewModel, Func<object, bool> canExecute, Action<object> execute)
        {
            m_CanExecuteFunc = canExecute;
            m_ExecuteAction = execute;
            hostViewModel.PropertyChanged += (sender, e) => CanExecuteChanged?.Invoke(hostViewModel, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => m_CanExecuteFunc(parameter);

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                m_ExecuteAction(parameter);
            }
        }
    }
}
