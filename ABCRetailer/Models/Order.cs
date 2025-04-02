namespace ABCRetailer.Models
{
    public class Order
    {
            public string OrderId { get; set; }
            public string CustomerName { get; set; }
            public List<Product> Products { get; set; }
            public decimal TotalPrice { get; set; }
    }
}
