using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public abstract class ValidationViewModel : AbstractViewModel, INotifyDataErrorInfo
    {
        private Dictionary<String, List<String>> errors = new Dictionary<String, List<String>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private Object threadLock = new Object();

        public Boolean IsValid
        {
            get
            {
                return !this.HasErrors;
            }
        }

        public void OnErrorsChanged(String propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public bool HasErrors
        {
            get
            {
                return errors.Any(propErrors => propErrors.Value != null && propErrors.Value.Count > 0);
            }
        }

        public ValidationViewModel() : base()
        {

        }

        protected override void initRelayCommands()
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            // Return the Error strings for the specified property
            if(!String.IsNullOrEmpty(propertyName))
            {
                if (errors.ContainsKey(propertyName) && (errors[propertyName] != null) && errors[propertyName].Count > 0)
                {
                    return errors[propertyName].ToList();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                // If no property given return all errors
                return errors.SelectMany(err => err.Value.ToList());
            }
        }

        public void ValidateProperty(Object value, [CallerMemberName] String propertyName = null)
        {
            lock(threadLock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var ValidationResults = new List<ValidationResult>();
                validationContext.MemberName = propertyName;

                // Try to validate the property 
                Validator.TryValidateProperty(value, validationContext, ValidationResults);

                // Remove the property from the error list and notify the binding
                if (errors.ContainsKey(propertyName))
                {
                    errors.Remove(propertyName);
                }
                OnErrorsChanged(propertyName);

                HandleValidationResults(ValidationResults);
            }
        }

        public void Validate()
        {
            var validationContext = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, validationResults, true);

            // Clear previous errors and notify the binding
            var propertyNames = errors.Keys.ToList();
            errors.Clear();
            propertyNames.ForEach(propName => OnErrorsChanged(propName));

            HandleValidationResults(validationResults);
        }

        private void HandleValidationResults(List<ValidationResult> validationResults)
        {
            // Group the results on property names
            var resultsByPropNames = from res in validationResults
                                     from mname in res.MemberNames
                                     group res by mname into g
                                     select g;

            // Add the errors to the dictionary and notify the binding with the errors
            foreach (var property in resultsByPropNames)
            {
                var messages = property.Select(r => r.ErrorMessage).ToList();
                errors.Add(property.Key, messages);
                OnErrorsChanged(property.Key);
            }
        }
    }
}
