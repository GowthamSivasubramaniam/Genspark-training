using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Twitter.Models
{
    public class HashtagPost
    {
      
        public int PostId { get; set; }
        public int HashtagId { get; set; }


        [ForeignKey("PostId")]
        public Post? Post { get; set; }
        [ForeignKey("HashtagId")]
        public Hashtag? Hashtag { get; set; }

    }
}