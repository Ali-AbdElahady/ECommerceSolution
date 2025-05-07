

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsShipped { get; set; } = false;
        public string CustomerId { get; set; } // Foreign key to ApplicationUser
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
