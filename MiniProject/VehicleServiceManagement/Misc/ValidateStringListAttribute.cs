using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VSM.Misc
{
    public class ValidateStringListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IEnumerable;
            if (list == null)
                return ValidationResult.Success;

            foreach (var item in list)
            {
                if (item is string str)
                {
                    if (!Regex.IsMatch(str, @"^[A-Za-z\s]{2,}$"))
                        return new ValidationResult("Each category name must be at least 2 letters and only contain letters and spaces.");
                }
            }
            return ValidationResult.Success;
        }
    }
}