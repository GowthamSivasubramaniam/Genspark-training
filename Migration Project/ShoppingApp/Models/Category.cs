

namespace ShoppingApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Category
    {
    [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Product>? Products { get; set; }
    }
}
