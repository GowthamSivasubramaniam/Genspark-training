using VSM.Models;
using VSM.DTO;
using System.Linq;
using System.Collections.Generic;

namespace VSM.Misc.Mappers
{
    public class BillMapper
    {
public BillDisplayDto MapToDisplayDto(Bill bill)
{
    return new BillDisplayDto
    {
        BillID = bill.BillID,
        Description = bill.Description,
        Status = bill.Status,
        CustomerName = bill.ServiceRecord?.Customer?.Name,
        Email=bill.ServiceRecord?.Customer?.Email,
        Memail=bill.ServiceRecord?.Mechanic?.Email,
        Customer_PhoneNo = bill.ServiceRecord?.Customer?.Phone,
        MechanicName = bill.ServiceRecord?.Mechanic?.Name,
        Mechanic_PhoneNo = bill.ServiceRecord?.Mechanic?.Phone,
        VehicleNo = bill.ServiceRecord?.Service?.Vehicle?.VehicleNo,
        CategoryAmounts = bill.CategoryDetails?
            .Select(cd => new CategoryAmountDto
            {
                CategoryName = cd.CategoryName,
                Amount = cd.Amount
            }).ToList() ?? new List<CategoryAmountDto>(),
        TotalAmount = bill.CategoryDetails?.Sum(cd => cd.Amount) ?? 0
    };
}


        public IEnumerable<BillDisplayDto> MapToDisplayDtos(IEnumerable<Bill> bills)
        {
            return bills.Select(MapToDisplayDto);
        }
    }
}