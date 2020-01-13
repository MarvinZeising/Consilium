using System.ComponentModel.DataAnnotations;

namespace Entities.Validators
{
    public class ValidPrivilegeAttribute : ValidationAttribute
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

            return valueAsString == "publisher"
                || valueAsString == "auxiliary"
                || valueAsString == "regular"
                || valueAsString == "special"
                || valueAsString == "circuit"
                || valueAsString == "bethelite"
                || valueAsString == "construction";
        }
    }
}
