using Backend.DTO;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly VideoService _videoService;

        public VideoController(VideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo([FromForm] VideoAddDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var video = await _videoService.addVideo(dto);
            return CreatedAtAction(nameof(GetVideos), new { id = video.Id }, video);
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideos([FromQuery] string? query)
        {
            var videos = await _videoService.GetVideos(query ?? "");
            return Ok(videos);
        }
    }
}
