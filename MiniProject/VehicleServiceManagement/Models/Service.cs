using System.ComponentModel.DataAnnotations;

namespace VSM.Models
{
    public class Service
    {
        [Key]
        public Guid ServiceID { get; set; } = Guid.NewGuid();
        public Guid VehicleID { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();
        public Vehicle? Vehicle { get; set; }
        public ServiceRecord? ServiceRecord { get; set; }


    }


}