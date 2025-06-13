using System.ComponentModel.DataAnnotations;

namespace VSM.DTO
{
    public class ServiceRecordStatusUpdateDto
    {
        [Required]
        public Guid ServiceRecordID { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}