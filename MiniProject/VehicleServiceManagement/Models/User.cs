using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public byte[]? Password { get; set; }
        public byte[]? HashKey { get; set; }
        public bool IsActive { get; set; } = false;
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime TTL { get; set; } = DateTime.UtcNow;

        public Customer? Customer { get; set; }
        public Mechanic? Mechanic { get; set; }

    }
}