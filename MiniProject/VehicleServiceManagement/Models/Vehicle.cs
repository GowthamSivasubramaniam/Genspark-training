using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class Vehicle
    {
        [Key]
        public Guid  VehicleID { get; set; } = Guid.NewGuid();
        public string VehicleNo { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string VechicleManufacturer { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Service> Services { get; set; } = new List<Service>();
        
    }
}