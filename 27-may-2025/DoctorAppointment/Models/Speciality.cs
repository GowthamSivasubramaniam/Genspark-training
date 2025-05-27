using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DocApp.Models
{
    public class Speciality
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }
    }
}