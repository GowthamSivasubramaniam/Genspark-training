


namespace ShoppingApp.Dtos
{
    public class NewsDto
    {

        public int? UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}