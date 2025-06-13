using System;

namespace VSM.DTO
{
    public class ServiceDisplayDto
    {
        public Guid ServiceID { get; set; }
        public Guid VehicleID { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
        public bool IsDeleted { get; set; }
    }
}