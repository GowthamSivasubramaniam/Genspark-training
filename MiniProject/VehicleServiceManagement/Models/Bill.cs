using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class Bill
    {
        [Key]
        public Guid BillID { get; set; } = Guid.NewGuid();
        public Guid ServiceRecordID { get; set; }
        public string Description { get; set; } = string.Empty;
        public float Amount { get; set; }
        public ServiceRecord? ServiceRecord { get; set; }
        public ICollection<BillCategoryDetail> CategoryDetails { get; set; } = new List<BillCategoryDetail>();


    }
}