using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RunApproachStatistics.Services.Validators
{
    class FloatValidator : ValidationRule
    {
        private String errorMessage;
        public String ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            float outFloat = 0;
            Boolean isFloat = float.TryParse((String)value, out outFloat);

            if (!isFloat)
            {
                result = new ValidationResult(false, this.ErrorMessage);
            }

            return result;
        }
    }
}
