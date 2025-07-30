

using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models
{
        public partial class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
     
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
