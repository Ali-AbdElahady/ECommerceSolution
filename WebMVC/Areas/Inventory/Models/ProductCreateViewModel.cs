using Application.DTOs.Category;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMVC.Areas.Inventory.Models
{
    public class ProductCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new();
        public List<IFormFile>? Images { get; set; }
        public List<ProductOptionInputModel> Options { get; set; } = new();
    }
}
