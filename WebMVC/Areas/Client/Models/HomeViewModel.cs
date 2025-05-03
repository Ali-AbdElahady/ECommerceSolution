using Application.Common.Models;
using Application.DTOs.Category;
using Application.DTOs.Product;

namespace WebMVC.Areas.Client.Models
{
    public class HomeViewModel
    {
        public List<ProductDto> FeaturedProducts { get; set; }
        public List<ProductCategoryDto> Categories { get; set; }
    }
}
