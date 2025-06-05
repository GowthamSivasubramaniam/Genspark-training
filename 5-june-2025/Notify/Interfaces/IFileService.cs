
using Notify.Models;

namespace Notify.Interfaces
{
    public interface IFilesService
    {
        Task<Files> UploadFileAsync(IFormFile file, string umail);
        Task<(byte[] content, string contentType)> DownloadFileAsync(string fileName);
        Task<IEnumerable<Files>> GetUserFilesAsync(string umail);
    }
}
