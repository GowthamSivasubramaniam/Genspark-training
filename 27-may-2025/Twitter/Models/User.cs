using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Twitter.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Bio { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Post>? Posts { get; set; }
        [InverseProperty("Follower")]
        public ICollection<Follow> Following? { get; set; } = new List<Follow>();

        [InverseProperty("Followed")]
        public ICollection<Follow> Followers? { get; set; } = new List<Follow>();
    }
}