using System.ComponentModel.DataAnnotations;

namespace VSM.DTO
{
    public class BookingAddDto
    {
        [Required]
        public Guid CustomerID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Slot must be less than 100 characters.")]
        public string Slot { get; set; } = string.Empty;
        [Required(ErrorMessage = "Image is required.")]
        public IFormFile? Image { get; set; }
    }

    public class BookingDisplayDto
    {
        public Guid BookingID { get; set; }
        public Guid CustomerID { get; set; }
        public DateTime BookedAt { get; set; }
        public string Slot { get; set; } = string.Empty;
        public DateTime DeliveryTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}