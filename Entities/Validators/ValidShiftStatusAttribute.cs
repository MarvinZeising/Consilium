using System.ComponentModel.DataAnnotations;

namespace Server.Entities.Validators
{
    public class ValidShiftStatusAttribute : ValidationAttribute
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

            return valueAsString == "draft" ||
                valueAsString == "pending" ||
                valueAsString == "planned" ||
                valueAsString == "suspense" ||
                valueAsString == "calledOff";
        }
    }
}
