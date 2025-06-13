using VSM.DTO;

namespace VSM.Interfaces
{
    public interface IBillService
    {
        Task<BillDisplayDto> Add(BillAddDto dto);
        Task<BillDisplayDto?> Get(Guid billId);
        Task<IEnumerable<BillDisplayDto>> GetAll();
        Task<IEnumerable<BillDisplayDto>> GetByServiceRecordId(Guid serviceRecordId);
        Task<byte[]> DownloadBillPdf(Guid billId);
    }
}