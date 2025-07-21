using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty; 
  }
}