using System.ComponentModel.DataAnnotations;

namespace Entities.Validators
{
    public class ValidAssignmentAttribute : ValidationAttribute
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
                || valueAsString == "ministerial"
                || valueAsString == "elder"
                || valueAsString == "cobe"
                || valueAsString == "secretary"
                || valueAsString == "serviceOverseer";
        }
    }
}
