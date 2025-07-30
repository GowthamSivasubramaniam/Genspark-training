

namespace Backend.DTO
{
    public class VideoAddDto
    {
       
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }=string.Empty;
       public IFormFile? Video
        { get; set; }
    }
}