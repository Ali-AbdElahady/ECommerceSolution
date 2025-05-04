namespace WebMVC.Areas.Client.Models
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Size { get; set; }
        public int OptionId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
