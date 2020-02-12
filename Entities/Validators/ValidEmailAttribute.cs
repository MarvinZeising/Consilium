using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Server.Entities.Validators
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

            return valueAsString.Length == 0 ||
                (Regex.IsMatch(valueAsString, ".+@.+\\..+") &&
                    valueAsString.Length <= 100);
        }
    }
}
