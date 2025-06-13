using System.ComponentModel.DataAnnotations;

namespace VSM.DTO
{
    public class ServiceRecordAddDto
    {
         [Required]
        public Guid MechanicId { get; set; }
         [Required]
        public Guid CustomerID { get; set; }
         [Required]
        public Guid ServiceID { get; set; }
         [Required]
        public Guid BookingID { get; set; }
    }
}