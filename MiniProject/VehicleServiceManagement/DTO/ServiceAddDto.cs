using System;
using System.ComponentModel.DataAnnotations;

namespace VSM.DTO
{
    public class ServiceAddDto
    {
        [Required]
        public Guid VehicleID { get; set; }
        public string Description { get; set; } = string.Empty;
         [Required]
        public List<string> CategoryNames { get; set; } = new();
    }
}