using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Validators
{
    public class ValidTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (!(value is int valueAsInt))
            {
                return false;
            }

            return valueAsInt >= 0
                && valueAsInt < 2400;
        }
    }
}
