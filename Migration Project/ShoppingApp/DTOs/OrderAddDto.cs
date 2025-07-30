namespace ShoppingApp.Dtos
{
    public class OrderDto
    {

        public string OrderName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string PaymentType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
    }

    public class OrderDetailDto
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }

    public class CreateOrderDto
    {
        public string OrderName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string PaymentType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;

        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
    public class GetOrderDto : CreateOrderDto
{
    public int OrderID { get; set; }
    public List<GetOrderDetailDto> OrderDetails { get; set; } = new();
}

public class GetOrderDetailDto : OrderDetailDto
{
    public string ProductName { get; set; } = string.Empty;
}

}

