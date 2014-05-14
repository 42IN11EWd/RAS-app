using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MvvmValidation;
using System.ComponentModel;

namespace RunApproachStatistics.ViewModel
{
    public abstract class ValidationViewModel : AbstractViewModel, INotifyDataErrorInfo
    {
        protected MvvmValidation.ValidationHelper Validator { get; private set; }
        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; set; }

		public ValidationViewModel() : base()
		{
            Validator = new ValidationHelper();
            NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
		}

        protected override void initRelayCommands()
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return NotifyDataErrorInfoAdapter.GetErrors(propertyName);
        }

        public String[] GetErrorArr(string propertyName)
        {
            String[] errors = (String[])NotifyDataErrorInfoAdapter.GetErrors(propertyName);
            if (errors.Length == 0)
            {
                errors = null;
            }
            return errors;
        }

        public bool HasErrors
        {
            get { return NotifyDataErrorInfoAdapter.HasErrors; }
        }
 
        public bool IsValid
        {
            get { return !HasErrors; }
        }

        public void ValidateAll()
        {
            Validator.ValidateAll();
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { NotifyDataErrorInfoAdapter.ErrorsChanged += value; }
            remove { NotifyDataErrorInfoAdapter.ErrorsChanged -= value; }
        }
    }
}
