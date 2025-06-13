using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class ServiceCategory
    {
    [Key]
    public Guid CategoryID { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
    public float Amount { get; set; } 
    public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}