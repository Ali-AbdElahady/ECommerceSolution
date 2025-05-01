
namespace Application.DTOs.Product
{
    public class ProductFilterDto
    {
        public string? Title { get; set; }
        public int? CategoryId { get; set; }

        public string? SortBy { get; set; } = "Title"; // Default sort
        public bool SortDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
