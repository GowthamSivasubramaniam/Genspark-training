using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Notify.Interfaces;
using Notify.Models;
using Notify.Misc;

namespace Notify.Services
{
    public class FilesService : Hub, IFilesService
    {
        private readonly IRepository<string, Files> _repo;
        private readonly IHubContext<NotificationHub> _hub;


        public FilesService(IRepository<string, Files> repo ,  IHubContext<NotificationHub> hub)
        {
            _repo = repo;
            _hub = hub;
        }

        public async Task<Files> UploadFileAsync(IFormFile file, string umail)
        {
            var fileName = Path.GetFileName(file.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileRecord = new Files
            {
                Umail = umail,
                FileName = fileName,
                FilePath = filePath
            };
            await _hub.Clients.All.SendAsync("Notification", umail, $"Added {filePath}");
            var Newfile = await _repo.Add(fileRecord);
            return Newfile;
        }

        public async Task<(byte[] content, string contentType)> DownloadFileAsync(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName);

            if (!File.Exists(path))
                throw new FileNotFoundException();

            var content = await File.ReadAllBytesAsync(path);
            var contentType = "application/octet-stream";

            return (content, contentType);
        }

        public async Task<IEnumerable<Files>> GetUserFilesAsync(string umail)
        {
            return await _repo.GetAll();
        }
        
         
    }
}
