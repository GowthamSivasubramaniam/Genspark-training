using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class Mechanic
    {
        [Key]
        public Guid MechanicId { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public ICollection<ServiceRecord>? ServiceRecords { get; set; }
        public User? User { get; set; }
    }


}