using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VSM.Misc
{
    public class CustomValidationAttribute : ValidationAttribute
    {
        public enum ValidationType
        {
            Name,
            Email,
            Phone,
            Password
        }

        private readonly ValidationType _type;

        public CustomValidationAttribute(ValidationType type)
        {
            _type = type;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult($"{validationContext.DisplayName} is required.");

            string strValue = value.ToString();

            switch (_type)
            {
                case ValidationType.Name:
                    if (!Regex.IsMatch(strValue, @"^[A-Za-z\s]{2,}$"))
                        return new ValidationResult("Invalid name format.");
                    break;
                case ValidationType.Email:
                    if (!Regex.IsMatch(strValue, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                        return new ValidationResult("Invalid email format.");
                    break;
                case ValidationType.Phone:
                    if (!Regex.IsMatch(strValue, @"^(\+?\d{1,3}[- ]?)?\d{10}$"))
                        return new ValidationResult("Invalid phone number format.");
                    break;
                 case ValidationType.Password:
                    if (strValue.Length < 6 ||
                        !Regex.IsMatch(strValue, @"[A-Z]") ||     // at least 1 uppercase
                        !Regex.IsMatch(strValue, @"[a-z]") ||     // at least 1 lowercase
                        !Regex.IsMatch(strValue, @"\d") ||        // at least 1 digit
                        !Regex.IsMatch(strValue, @"[\W_]"))       // at least 1 symbol (non-word character)
                    {
                        return new ValidationResult("Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                    }
                    break;
                   
            }

            return ValidationResult.Success;
        }
    }
}