

namespace ShoppingApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public virtual ICollection<News>? News { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
