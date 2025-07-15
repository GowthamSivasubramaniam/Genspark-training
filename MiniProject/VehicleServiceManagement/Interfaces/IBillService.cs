using VSM.DTO;
using VSM.Models;

namespace VSM.Interfaces
{
    public interface IBillService
    {
        Task<BillDisplayDto> Add(BillAddDto dto);
        Task<BillDisplayDto?> Get(Guid billId);
        Task<IEnumerable<BillDisplayDto>> GetAll(int page, int pageSize, string? search = null);
        Task<BillDisplayDto?> Update(Guid billId,string status );
        Task<Bill?> Delete(Guid billId);
        Task<IEnumerable<BillDisplayDto>> GetByServiceRecordId(Guid serviceRecordId);
        Task<byte[]> DownloadBillPdf(Guid billId);
    }
}