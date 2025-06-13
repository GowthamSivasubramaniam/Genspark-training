using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VSM.Misc;
namespace VSM.DTO
{
    public class ServiceCategoryUpdateDto
    {
        [Required]
        [ValidateStringList]
        public List<string> CategoryNames { get; set; } = new();
    }
}