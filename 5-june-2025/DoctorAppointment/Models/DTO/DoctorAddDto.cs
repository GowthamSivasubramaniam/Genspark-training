using DoctorAppointment.Misc;

namespace DoctorAppointment.Models.DTO
{
    public class DoctorAddDto
    {
     [NameValidation]
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Email { get; set; }=string.Empty;
     public string Password { get; set; } = string.Empty;
    public float YearsOfExperience { get; set; }
        public ICollection<SpecialityAddDto>? Specialities { get; set; }

    }
}