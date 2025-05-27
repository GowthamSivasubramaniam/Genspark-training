using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Twitter.Models
{
    public class Likes
    {
         [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("PostId")]
        public Post? Post { get; set; }
        public Likes()
        {
            CreatedAt = DateTime.Now;
        }
    }
}