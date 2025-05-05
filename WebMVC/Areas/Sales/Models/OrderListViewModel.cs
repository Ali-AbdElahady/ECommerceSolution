namespace WebMVC.Areas.Sales.Models
{
    public class OrderListViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsShipped { get; set; }

        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerUserName { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public List<OrderItemViewModel> Items { get; set; } = new();
    }
}
