using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notify.Interfaces;
using Notify.Models;

namespace Notify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesService _filesService;

        public FilesController(IFilesService filesService)
        {
            _filesService = filesService;
        }

        [HttpPost("upload")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UploadFile(IFormFile file, string umail)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file selected");

            var savedFile = await _filesService.UploadFileAsync(file, umail);
            return Ok(savedFile);
        }

        [HttpGet("download/{filename}")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            try
            {
                var (content, contentType) = await _filesService.DownloadFileAsync(filename);
                return File(content, contentType, filename);
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found");
            }
        }

        // [HttpGet("user/{umail}")]
        // public async Task<IActionResult> GetFilesByUser(string umail)
        // {
        //     var files = await _filesService.GetUserFilesAsync(umail);
        //     return Ok(files);
        // }
    }
}
