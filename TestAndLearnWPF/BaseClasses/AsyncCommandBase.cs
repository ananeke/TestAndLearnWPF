using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BaseClassesLibrary.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        #region Variables and Properties
        protected bool isExecuting;
        /// <summary>
        /// Occurs when the command is executing.
        /// </summary>
        public bool IsExecuting
        {
            get
            {
                return isExecuting;
            }
            private set
            {
                isExecuting = value;
                OnExecuteChanged?.Invoke(isExecuting);
            }
        }

        /// <summary>
        /// Fired when the command is executing or finish execution
        /// </summary>
        public event Action<bool> OnExecuteChanged;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        public AsyncCommandBase(Action<bool> onExecuteChanged = null)
        {
            IsExecuting = false;
            if (onExecuteChanged != null)
                OnExecuteChanged += onExecuteChanged;
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public virtual bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public virtual async void Execute(object parameter)
        {
            try
            {
                IsExecuting = true;
                await OnExecute(parameter);
            }
            finally
            {
                IsExecuting = false;
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected abstract Task OnExecute(object parameter);
    }
}
