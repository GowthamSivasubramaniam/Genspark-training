namespace VSM.DTO
{
    public class VehicleAdd
    {
        public string VehicleNo { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string VechicleManufacturer { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
    }

    public class VehicleDisplayDto
    {
        public Guid VehicleID { get; set; }
        public string VehicleNo { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string VechicleManufacturer { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
    }
}
