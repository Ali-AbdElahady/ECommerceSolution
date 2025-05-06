using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMVC.Areas.Inventory.Models
{
    public class ProductOptionInputModel
    {
        public string Size { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }

    }
}
