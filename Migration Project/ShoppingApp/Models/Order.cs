

using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models
{

    public partial class Order
    {
        [Key]
        public int OrderID { get; set; }
        public string OrderName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string PaymentType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
