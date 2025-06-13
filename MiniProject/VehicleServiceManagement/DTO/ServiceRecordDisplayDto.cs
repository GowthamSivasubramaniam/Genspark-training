namespace VSM.DTO
{
    public class ServiceRecordDisplayDto
    {
        public Guid ServiceRecordID { get; set; }
        public Guid MechanicId { get; set; }
        public Guid CustomerID { get; set; }
        public Guid ServiceID { get; set; }
        public Guid BookingID { get; set; }
        public string Status { get; set; } = string.Empty;
        // public bool IsDeleted { get; set; }
    }
}