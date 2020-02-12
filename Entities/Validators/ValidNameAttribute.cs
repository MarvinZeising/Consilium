using System.ComponentModel.DataAnnotations;

namespace Server.Entities.Validators
{
    public class ValidNameAttribute : ValidationAttribute
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

            return valueAsString.Trim().Length >= 2 &&
                valueAsString.Length <= 40;
        }
    }
}
