using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DocApp.Models
{
    public class Appointmnet
    {
        [Key]
        public string Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmnetDateTime { get; set; }
       
        public string Status { get; set; } = string.Empty;
        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }
         [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }
    }
}