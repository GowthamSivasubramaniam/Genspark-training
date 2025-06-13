using Microsoft.EntityFrameworkCore;
using VSM.Models;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc;
using VSM.Misc.Mappers;

namespace VSM.Services
{
    public class BillService : IBillService
    {
        private readonly IRepository<Guid, Bill> _repo;
        private readonly IRepository<Guid, ServiceRecord> _serviceRecordRepo;
        private readonly BillMapper _mapper = new();

        public BillService(IRepository<Guid, Bill> repo, IRepository<Guid, ServiceRecord> serviceRecordRepo)
        {
            _repo = repo;
            _serviceRecordRepo = serviceRecordRepo;
        }

        public async Task<BillDisplayDto> Add(BillAddDto dto)
        {
            var serviceRecord = await _serviceRecordRepo.Get(dto.ServiceRecordID);
            if (serviceRecord == null) throw new Exception("Service record not found");

            var categories = serviceRecord.Service?.ServiceCategories ?? new List<ServiceCategory>();

            var bill = new Bill
            {
                ServiceRecordID = dto.ServiceRecordID,
                Description = dto.Description,
                CategoryDetails = new List<BillCategoryDetail>()
            };

            // Add all service categories with their amounts
            foreach (var cat in categories)
            {
                bill.CategoryDetails.Add(new BillCategoryDetail
                {
                    CategoryName = cat.Name,
                    Amount = cat.Amount
                });
            }

            // Always add a Misc category (0 if not provided)
            float miscAmount = dto.MiscAmount ?? 0;
            bill.CategoryDetails.Add(new BillCategoryDetail
            {
                CategoryName = "Misc",
                Amount = miscAmount
            });

            // Calculate total
            bill.Amount = bill.CategoryDetails.Sum(c => c.Amount);

            var added = await _repo.Add(bill) ?? throw new Exception("Unable to add bill");

            // Use the mapper for consistency
            return _mapper.MapToDisplayDto(added);
        }

        public async Task<BillDisplayDto?> Get(Guid billId)
        {
            var bill = await _repo.Get(billId);
             if (bill == null)
            {
                throw new Exception("No Bills found");
            }
            return bill != null ? _mapper.MapToDisplayDto(bill) : null;
        }

        public async Task<IEnumerable<BillDisplayDto>> GetAll()
        {
            var bills = await _repo.GetAll(1, 100);
             if (bills.Count()==0)
            {
                throw new Exception("No Bills found");
            }
            return _mapper.MapToDisplayDtos(bills);
        }

        public async Task<IEnumerable<BillDisplayDto>> GetByServiceRecordId(Guid serviceRecordId)
        {
            var bills = (await _repo.GetAll(1, 100)).Where(b => b.ServiceRecordID == serviceRecordId);
            if (bills.Count()==0)
            {
                throw new Exception("No Bills found");
            }
            return _mapper.MapToDisplayDtos(bills);
        }

        public async Task<byte[]> DownloadBillPdf(Guid billId)
        {
            var bill = await _repo.Get(billId);
            
            var customerName = bill.ServiceRecord?.Customer?.Name ?? "Unknown";
            var serviceDescription = bill.ServiceRecord?.Service?.Description ?? "N/A";
            var billDescription = bill.Description;
            var amount = bill.Amount;

            
            var categoryAmounts = bill.CategoryDetails
                .Select(cd => new CategoryAmountDto
                {
                    CategoryName = cd.CategoryName,
                    Amount = cd.Amount
                })
                .ToList();

            return BillPdfHelper.GenerateBillPdf(
                bill.BillID,
                customerName,
                serviceDescription,
                categoryAmounts,
                billDescription,
                amount
            );
        }
    }
}
