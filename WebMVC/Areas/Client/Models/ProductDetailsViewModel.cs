using Application.DTOs.Product;

namespace WebMVC.Areas.Client.Models
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public List<string> ImagePath { get; set; } = new List<string>();
        public int ProductCategoryId { get; set; }
        public List<ProductOptionDto> Options { get; set; } = new List<ProductOptionDto>();
    }
}
