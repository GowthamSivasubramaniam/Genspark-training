namespace DoctorAppointment.Models.DTO
{
    public class DoctorAddDto
    {
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public float YearsOfExperience { get; set; }
        public ICollection<SpecialityAddDto>? Specialities { get; set; }

    }
}