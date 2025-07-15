namespace VSM.DTO
{
    public class ServiceRecordDisplayDto
    {
        public Guid ServiceRecordID { get; set; }
        public Guid MechanicId { get; set; }
        public Guid CustomerID { get; set; }
        public Guid ServiceID { get; set; }
        public Guid BookingID { get; set; }
        public string CustomerName { get; set; }
        public string Customer_Email { get; set; }
        public string Customer_PhoneNo { get; set; }
        public string MechanicName { get; set; }
        public string Mechanic_Email { get; set; }
        public string Mechanic_PhoneNo { get; set; }
        public string[] Categories { get; set; }
        public string VehicleNo { get; set; }
        public string Description{ get; set; }

        public string Status { get; set; } = string.Empty;
            // public int TotalCount { get; set; }

    }
}