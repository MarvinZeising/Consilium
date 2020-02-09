using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities.Validators
{
    public class ValidDateAttribute : ValidationAttribute
    {
        public override bool IsValid (object value)
        {
            if (value == null)
            {
                return false;
            }

            if (!(value is int valueAsInt))
            {
                return false;
            }

            return valueAsInt > 20000000 &&
                valueAsInt < 20250000;
        }
    }
}
