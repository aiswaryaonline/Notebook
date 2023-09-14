using System;
using System.Windows.Input;

namespace HandNote.ViewModels
{
    /// <summary>
    /// The class implementes the ICommand
    /// </summary>
    public class Command : ICommand
    {
        private Action<object> _action;
        private Func<bool> _canExecuteEvaluator;

        public Command (Action<object> action, Func<bool> canExecuteEvaluator = null)
        {
            _action = action;
            _canExecuteEvaluator = canExecuteEvaluator;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecuteEvaluator == null)
            {
                return true;
            }
            else
            {
                bool result = _canExecuteEvaluator.Invoke();
                return result;
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
