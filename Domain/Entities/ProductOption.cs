
namespace Domain.Entities
{
    public class ProductOption
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
