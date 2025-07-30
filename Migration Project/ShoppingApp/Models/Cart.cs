

using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        
        public Product Product { get; set; }
        public int Quantity { get; set; }
#pragma warning disable CS8618 
        public Cart() { }
#pragma warning restore CS8618 
        public Cart(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}