
namespace Application.DTOs.Stock
{
    public class ReserveStockDto
    {
        public int ProductOptionId { get; set; }
        public int Quantity { get; set; }
        public int? OrderId { get; set; }
    }
}
