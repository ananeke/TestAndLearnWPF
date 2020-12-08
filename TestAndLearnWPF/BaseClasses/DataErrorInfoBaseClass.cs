using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClassesLibrary.ViewModels
{
    public abstract class DataErrorInfoBaseClass : BaseViewModel
    {
        protected object _classlock;
        /// <summary>
        /// Properties names that need to be validated
        /// </summary>
        protected abstract List<string> PropertyNames { get; set; }
        protected Dictionary<string, List<string>> errorMessages;

        #region Constructors
        public DataErrorInfoBaseClass()
        {
            _classlock = new object();
            ErrorMessagesInitialize();
        }
        #endregion

        #region Protected Methods
        protected void ErrorMessagesInitialize()
        {
            errorMessages = new Dictionary<string, List<string>>();
            foreach (var propertyName in PropertyNames)
            {
                errorMessages.Add(propertyName, new List<string>());
            }
        }

        /// <summary>
        /// Disposes the managed resources implementing <see cref="IDisposable"/>.
        /// </summary>
        protected override void DisposeManaged()
        {
            foreach (var property in errorMessages.Keys)
            {
                errorMessages[property].Clear();
            }
            errorMessages.Clear();
            errorMessages = null;
            _classlock = null;
        }
        #endregion
    }
}
