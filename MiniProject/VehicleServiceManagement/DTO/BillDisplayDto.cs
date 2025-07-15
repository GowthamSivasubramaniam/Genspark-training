using System;
using System.Collections.Generic;

namespace VSM.DTO
{
    public class BillDisplayDto
    {
        public Guid BillID { get; set; }
        public string? Email { get; set; }
        public string? Memail { get; set; }
        public string? CustomerName { get; set; }
        public string? Customer_PhoneNo { get; set; }
        public string? MechanicName { get; set; }
        public string? Mechanic_PhoneNo { get; set; }
        public string? VehicleNo { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public List<CategoryAmountDto> CategoryAmounts { get; set; } = new();
        public float TotalAmount { get; set; }
        //  public int TotalCount { get; set; }

    }

    public class CategoryAmountDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public float Amount { get; set; }
    }
}