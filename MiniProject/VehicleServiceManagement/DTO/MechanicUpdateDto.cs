

using System.ComponentModel.DataAnnotations;
using CustomValidationAttribute = VSM.Misc.CustomValidationAttribute;
namespace VSM.DTO
{
    public class MechanicUpdateDto
    {
        [Required]
         [CustomValidationAttribute(CustomValidationAttribute.ValidationType.Name)]
        public string Name { get; set; } = string.Empty;


        [Required]
        [CustomValidationAttribute(CustomValidationAttribute.ValidationType.Phone)]
        public string Phone { get; set; } = string.Empty;
    }
}