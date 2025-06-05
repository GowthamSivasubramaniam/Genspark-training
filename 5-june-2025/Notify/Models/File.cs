using System.ComponentModel.DataAnnotations;

namespace Notify.Models
{
    public class Files
    {
        [Key]
        public int FileId { get; set; }

        public string Umail { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public User? user { get; set; }
    }
}
