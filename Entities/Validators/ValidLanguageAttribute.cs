using System.ComponentModel.DataAnnotations;

namespace Entities.Validators
{
    public class ValidLanguageAttribute : ValidationAttribute
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

            return valueAsString == "en-US"
                || valueAsString == "de-DE";
        }
    }
}
