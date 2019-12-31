using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Entities.Validators
{
    public class ValidEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (!(value is string valueAsString))
            {
                return false;
            }

            return Regex.IsMatch(valueAsString, ".+@.+\\..+")
                && valueAsString.Length <= 100;
        }
    }
}
