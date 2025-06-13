using VSM.Models;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc.Mappers;

namespace VSM.Services
{
    public class ServiceRecordService : IServiceRecordService
    {
        private readonly IRepository<Guid, ServiceRecord> _repo;
        private readonly IRepository<Guid, Customer> _customerRepo;
        private readonly IRepository<Guid, Mechanic> _mechanicRepo;
        private readonly IRepository<Guid, Service> _serviceRepo;
        private readonly IRepository<Guid, Booking> _bookingRepo;
        private readonly ServiceRecordMapper _mapper = new();

        public ServiceRecordService(
            IRepository<Guid, ServiceRecord> repo,
            IRepository<Guid, Customer> customerRepo,
            IRepository<Guid, Mechanic> mechanicRepo,
            IRepository<Guid, Service> serviceRepo,
            IRepository<Guid, Booking> bookingRepo)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _mechanicRepo = mechanicRepo;
            _serviceRepo = serviceRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<ServiceRecordDisplayDto> Add(ServiceRecordAddDto dto)
        {
            
            var customer = await _customerRepo.Get(dto.CustomerID);
            if (customer == null || customer.Status =="Deleted") throw new Exception("Customer not found");
            var mechanic = await _mechanicRepo.Get(dto.MechanicId);
            if (mechanic == null || mechanic.Status =="Deleted") throw new Exception("Mechanic not found");
            var service = await _serviceRepo.Get(dto.ServiceID);
            if (service == null) throw new Exception("Service not found");
            var booking = await _bookingRepo.Get(dto.BookingID);
            if(booking.IsDeleted || booking.Status == "Cancelled") throw new Exception("Booking Was Deleted or Cancelled");
            if (booking == null) throw new Exception("Booking not found");

            var record = _mapper.MapAddDto(dto);
            var added = await _repo.Add(record) ?? throw new Exception("Unable to add service record");
            return _mapper.MapToDisplayDto(added);
        }

        public async Task<ServiceRecordDisplayDto> UpdateStatus(ServiceRecordStatusUpdateDto dto)
        {
            var record = await _repo.Get(dto.ServiceRecordID);
            if (record == null) throw new Exception("Service record not found");
            record.Status = dto.Status;
            var updated = await _repo.Update(dto.ServiceRecordID, record) ?? throw new Exception("Unable to update status");
            return _mapper.MapToDisplayDto(updated);
        }

        public async Task<ServiceRecordDisplayDto?> Get(Guid serviceRecordId)
        {
            var record = await _repo.Get(serviceRecordId);
            if (record == null) throw new Exception("Service record not found");
            return _mapper.MapToDisplayDto(record);
        }

        public async Task<IEnumerable<ServiceRecordDisplayDto>> GetAll()
        {
            var records = await _repo.GetAll(1,100);
             if (records.Count() == 0) throw new Exception("No service records found");
            return _mapper.MapToDisplayDtos(records);
        }

        public async Task<IEnumerable<ServiceRecordDisplayDto>> GetByCustomerId(Guid customerId)
        {
            var records = (await _repo.GetAll(1,100)).Where(r => r.CustomerID == customerId);
            if (records.Count() == 0) throw new Exception("No service records found");
            return _mapper.MapToDisplayDtos(records);
        }

        public async Task<IEnumerable<ServiceRecordDisplayDto>> GetByMechanicId(Guid mechanicId)
        {
            var records = (await _repo.GetAll(1,100)).Where(r => r.MechanicId == mechanicId);
             if (records.Count() == 0) throw new Exception("No service records found");
            return _mapper.MapToDisplayDtos(records);
        }
    }
}