using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Twitter.Models
{
    public class Follow
    {
         [Key]
         public int Id { get; set; }
         public int FollowerId { get; set; }

        [Required]
        public int FollowedId { get; set; }


        [ForeignKey(nameof(FollowerId))]
        [InverseProperty("Following")]
        public User Follower { get; set; }

        [ForeignKey(nameof(FollowedId))]
        [InverseProperty("Followers")]
        public User Followed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Follow()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}