using System.ComponentModel.DataAnnotations;

namespace VSM.DTO
{
    public class BillAddDto
    {
        public Guid ServiceRecordID { get; set; }
        public float? MiscAmount { get; set; } // Optional, defaults to 0 if not provided
        public string? Description { get; set; }
    }
}