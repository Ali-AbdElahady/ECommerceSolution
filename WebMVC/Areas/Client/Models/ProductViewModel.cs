using Application.DTOs.Category;
using Application.DTOs.Product;

namespace WebMVC.Areas.Client.Models
{
    public class ProductViewModel
    {
        public List<ProductDto> Products { get; set; }
        public List<ProductCategoryDto> Categories { get; set; }
        public string? Title { get; set; }
        public int? CategoryId { get; set; }

        public string? SortBy { get; set; } = "Title"; // Default sort
        public bool SortDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Count { get; set; }
    }
}
