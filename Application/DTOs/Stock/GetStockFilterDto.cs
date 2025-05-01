

namespace Application.DTOs.Stock
{
    public class GetStockFilterDto
    {
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string? SearchKeyword { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}
