namespace WebMVC.Areas.Sales.Models
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
