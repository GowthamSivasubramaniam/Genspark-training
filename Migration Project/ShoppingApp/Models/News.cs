

using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models
{

    public partial class News
    {
      [Key]
        public int NewsId { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int Status { get; set; }
    
        public virtual User? User { get; set; }
    }
}
