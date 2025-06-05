using System.ComponentModel.DataAnnotations;

namespace Notify.Models
{


    public class User
    {
        [Key]
        public string Mail { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public byte[]? Password { get; set; }
        public byte[]? HashKey { get; set; }
        public ICollection<Files>? Files { get; set; }
      
    }
}