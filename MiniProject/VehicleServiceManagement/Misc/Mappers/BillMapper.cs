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