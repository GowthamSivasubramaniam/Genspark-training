using System.ComponentModel.DataAnnotations;

namespace VSM.DTO
{
    public class BillAddDto
    {
        public Guid ServiceRecordID { get; set; }
        public string Status = "Dispatched";
        public float? MiscAmount { get; set; } 
        public string? Description { get; set; }
    }
}