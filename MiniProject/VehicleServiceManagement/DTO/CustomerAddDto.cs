using System.ComponentModel.DataAnnotations;
using CustomValidationAttribute = VSM.Misc.CustomValidationAttribute;

namespace VSM.DTO
{
    public class CustomerAddDto
    {

        [Required]
         [CustomValidationAttribute(CustomValidationAttribute.ValidationType.Name)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [CustomValidationAttribute(CustomValidationAttribute.ValidationType.Email)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [CustomValidationAttribute(CustomValidationAttribute.ValidationType.Phone)]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [CustomValidation(CustomValidationAttribute.ValidationType.Password)]
        public string Password { get; set; } = string.Empty;

        
    }
}
