using System;
using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class BillCategoryDetail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CategoryName { get; set; } = string.Empty;
        public float Amount { get; set; }
        public Guid BillId { get; set; }
        public Bill? Bill { get; set; }
    }
}