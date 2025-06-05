using System.ComponentModel.DataAnnotations;

namespace Notify.Models.DTO
{  
    
    public class UserLoginRequest
    {

        [Required(ErrorMessage = "Email is manditory")]
        [MinLength(5, ErrorMessage = "Invalid entry for Email")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is manditory")]
        public string Password { get; set; } = string.Empty;
    }
}