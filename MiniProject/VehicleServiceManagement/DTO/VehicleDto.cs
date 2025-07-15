using System.ComponentModel.DataAnnotations;

namespace VSM.DTO
{
    public class VehicleAdd
    {
        [Required]
        public string VehicleNo { get; set; } = string.Empty;
         [Required]
        public string VehicleType { get; set; } = string.Empty;
         [Required]
        public string VechicleManufacturer { get; set; } = string.Empty;
         [Required]
        public string VehicleModel { get; set; } = string.Empty;
    }

    public class VehicleDisplayDto
    {
        public Guid VehicleID { get; set; }
        public string VehicleNo { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string VechicleManufacturer { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        //   public int TotalCount { get; set; }

    }
}
