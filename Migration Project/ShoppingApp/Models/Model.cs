

namespace ShoppingApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Model
    {
    
        public int ModelId { get; set; }
        public string Model1 { get; set; } = string.Empty;
    
       
        public virtual ICollection<Product>? Products { get; set; }
    }
}
