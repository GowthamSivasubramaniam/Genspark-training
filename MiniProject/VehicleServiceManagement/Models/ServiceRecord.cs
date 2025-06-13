using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class ServiceRecord
    {
        [Key]
        public Guid ServiceRecordID { get; set; } = Guid.NewGuid();
        [Required]
        public Guid MechanicId { get; set; }
        public Guid CustomerID { get; set; } 
        public Guid ServiceID { get; set; } 
        public Guid BookingID { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public Customer? Customer { get; set; }
        public Mechanic? Mechanic { get; set; }
        public Booking? Booking { get; set; }
        public Service? Service { get; set; }
        public Bill? Bill { get; set; }

    }


}