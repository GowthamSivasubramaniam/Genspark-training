

namespace ShoppingApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Color
    {
       [Key]
        public int ColorId { get; set; }
        public string Color1 { get; set; } = string.Empty;

        public virtual ICollection<Product>? Products { get; set; }
    }
}
