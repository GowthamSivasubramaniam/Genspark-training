namespace ShoppingApp.Dtos
{
    public class ProductDisplayDto
    {
     public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ColorName { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string ModelName { get; set; }
    }
}