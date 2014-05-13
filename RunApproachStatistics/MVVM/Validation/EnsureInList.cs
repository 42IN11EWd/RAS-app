using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.MVVM.Validation
{
    class EnsureInList : ValidationAttribute
    {
        private readonly IList _listToCheck;
        public EnsureInList(IList listToCheck)
        {
            _listToCheck = listToCheck;
        }

        public override bool IsValid(object value)
        {
            return _listToCheck.Contains(value);
        }
    }
}
