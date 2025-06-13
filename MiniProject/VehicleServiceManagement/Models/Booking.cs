using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class Booking
    {
        [Key]
        public Guid BookingID { get; set; } = Guid.NewGuid();
        public Guid CustomerID{ get; set; } 
        public DateTime BookedAt { get; set; } = DateTime.UtcNow;
        public string Imageurl { get; set; } = string.Empty;
        public DateTime Slot { get; set; } = DateTime.UtcNow;
        public DateTime DeliveryTime { get; set; } = DateTime.UtcNow.AddDays(2);
        public string Status { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public ServiceRecord? ServiceRecord { get; set; }
        public Customer? Customer { get; set; }
        
    }
}