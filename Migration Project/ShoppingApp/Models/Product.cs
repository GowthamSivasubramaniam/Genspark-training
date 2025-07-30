

namespace ShoppingApp.Models
{
   
    
    public partial class Product
    {
    
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Price { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int ColorId { get; set; }
        public int ModelId { get; set; }
        public int StorageId { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime SellEndDate { get; set; }
        public int IsNew { get; set; }
    
        public virtual Category? Category { get; set; }
        public virtual Color? Color { get; set; }
        public virtual Model? Model { get; set; }
              public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual User? User { get; set; }
    }
}
