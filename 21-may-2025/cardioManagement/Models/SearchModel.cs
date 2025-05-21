namespace cardioManagement.Models
{
    public class SearchModel
    {
        public int? Id { get; set; }
        public string? PatientName { get; set; }
        public int? PatientAge { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Reason { get; set; }
    }
}
