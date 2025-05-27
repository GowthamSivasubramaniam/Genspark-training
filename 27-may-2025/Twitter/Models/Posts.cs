using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Twitter.Models
{
    public class Post
    {
         [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User? Author { get; set; }
        public Post()
        {
            CreatedAt = DateTime.Now;
        }
        public ICollection<Likes>? Likes { get; set; }
        public ICollection<HashtagPost>? Hashtags { get; set; }

    }
}