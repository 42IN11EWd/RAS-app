using RunApproachStatistics.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.MVVM.Validation
{
    [AttributeUsage(AttributeTargets.Class)]
    class EnsureInList : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var model = (MeasurementViewModel)value;
            return model.Locations.Contains(model.Location);
        }
    }
}
