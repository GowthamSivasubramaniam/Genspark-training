using Backend.Context;
using Backend.DTO;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend.Services
{


    public class VideoService
    {
        private readonly VideoContext _context;
        private readonly AzureBlobService _blobService;

        public VideoService(VideoContext context, AzureBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        public async Task<Video> addVideo(VideoAddDto dto)
        {

            var videoUrl = await _blobService.UploadAsync(dto.Video);
            var video = new Video
            {
                Name = dto.Name,
                Description = dto.Description,
                Url = videoUrl
            };
            await _context.AddAsync(video);
            await _context.SaveChangesAsync();
            return video;
        }
        public async Task<IEnumerable<Video>> GetVideos(string query)
        {
            var videosQuery = _context.Videos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
               videosQuery = videosQuery.Where(v =>
    v.Name.ToLower().Contains(query.ToLower()) ||
    v.Description.ToLower().Contains(query.ToLower()));

            }

            var videos = await videosQuery.ToListAsync();
            return videos;
        }



    }
}
