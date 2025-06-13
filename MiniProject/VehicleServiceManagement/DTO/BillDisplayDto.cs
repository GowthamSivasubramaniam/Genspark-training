using System;
using System.Collections.Generic;

namespace VSM.DTO
{
    public class BillDisplayDto
    {
        public Guid BillID { get; set; }
        public string? Description { get; set; }
        public List<CategoryAmountDto> CategoryAmounts { get; set; } = new();
        public float TotalAmount { get; set; }
    }

    public class CategoryAmountDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public float Amount { get; set; }
    }
}