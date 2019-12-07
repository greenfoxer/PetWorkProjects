namespace clientwpf
{
    using System;
    using System.Windows.Input;

    internal class Command : ICommand
    {
        #region Fields

        private readonly CommandOnCanExecute _canExecute;

        private readonly CommandOnExecute _execute;

        #endregion

        #region Constructors and Destructors

        public Command(CommandOnExecute onExecuteMethod, CommandOnCanExecute onCanExecuteMethod)
        {
            _execute = onExecuteMethod;
            _canExecute = onCanExecuteMethod;
        }

        #endregion

        #region Delegates

        public delegate bool CommandOnCanExecute(object parameter);

        public delegate void CommandOnExecute(object parameter);

        #endregion

        #region Public Events

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }

        #endregion
    }
}

