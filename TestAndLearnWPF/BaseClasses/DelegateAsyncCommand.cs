using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaseClassesLibrary.Commands
{
    public class DelegateAsyncCommand : AsyncCommandBase
    {
        /// <summary>
        /// Token to cancel async command.
        /// </summary>
        private CancellationTokenSource cancelToken;

        /// <summary>
        /// Can execute delegate.
        /// </summary>
        public Predicate<object> CanExecuteDelegate { get; private set; }

        /// <summary>
        /// Execute delegate.
        /// </summary>
        public Func<object, CancellationToken, Task> ExecuteDelegate { get; private set; }

        /// <summary>
        /// Cancel command.
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateAsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateAsyncCommand( Func<object, CancellationToken, Task> execute, Predicate<object> canExecute = null, Action<bool> onExecuteChanged = null) : base(onExecuteChanged)
        {
            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute ?? throw new ArgumentNullException( "Execute" );
            cancelToken = new CancellationTokenSource();
            CancelCommand = new DelegateCommand( (o) => cancelToken?.Cancel(), (o) => true );
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed(default if CanExecuteDelegate not set); otherwise, false.
        /// </returns>
        public override bool CanExecute( object parameter )
        {
            bool canExecute = false;
            try
            {
                canExecute = (CanExecuteDelegate != null) ? (!IsExecuting && CanExecuteDelegate(parameter)) : !IsExecuting;
            }
            catch
            {
                canExecute = false;
            }
            return canExecute;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        protected override async Task OnExecute( object parameter )
        {
            await ExecuteDelegate( parameter, cancelToken.Token );
        }
    }

    public class DelegateAsyncCommand<T> : AsyncCommandBase
    {
        /// <summary>
        /// Token to cancel async command.
        /// </summary>
        private CancellationTokenSource cancelToken;

        /// <summary>
        /// Can execute delegate.
        /// </summary>
        public Predicate<T> CanExecuteDelegate { get; private set; }

        /// <summary>
        /// Execute delegate.
        /// </summary>
        public Func<T, CancellationToken, Task> ExecuteDelegate { get; private set; }

        /// <summary>
        /// Cancel command.
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }

        protected bool isValueType;
        /// <summary>
        /// Occurs when the T type is value Type.
        /// </summary>
        public bool IsValueType
        {
            get
            {
                return isValueType;
            }
            private set
            {
                isValueType = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateAsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateAsyncCommand(Func<T, CancellationToken, Task> execute, Predicate<T> canExecute = null, Action<bool> onExecuteChanged = null) : base(onExecuteChanged)
        {
            Type nullableType = typeof(Nullable<>);
            Type tTypeInfo = typeof(T);

            // For value type can start delegate method only when parameter is not null
            if (tTypeInfo.IsValueType && (!tTypeInfo.IsGenericType || !nullableType.IsAssignableFrom(tTypeInfo)))
                isValueType = true;
            else
                isValueType = false;

            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute ?? throw new ArgumentNullException("Execute");
            cancelToken = new CancellationTokenSource();
            CancelCommand = new DelegateCommand((o) => cancelToken?.Cancel(), (o) => true);
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed(default if CanExecuteDelegate not set); otherwise, false.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            bool canExecute = false;
            try
            {
                bool delegateCanBeExecuted = CanExecuteDelegate != null;

                bool hasValueOrCanBeNull = !IsValueType || (IsValueType && parameter != null);

                canExecute = (delegateCanBeExecuted && hasValueOrCanBeNull) ? !IsExecuting && CanExecuteDelegate((T)parameter)
                           : (delegateCanBeExecuted && IsValueType && parameter == null) ? false : !IsExecuting;
            }
            catch
            {
                canExecute = false;
            }
            return canExecute;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        protected override async Task OnExecute(object parameter)
        {
            if (isValueType && parameter == null)
                await ExecuteDelegate(default(T), cancelToken.Token);
            else
                await ExecuteDelegate((T)parameter, cancelToken.Token);
        }
    }
}
