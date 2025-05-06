using Application.DTOs.Category;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Areas.Inventory.Controllers
{
    [Area("Category")]
    [Authorize(Roles = "InventoryManager")]
    [Route("Category/Product")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var filter = new CategoryFilterDto(); // Default filter (can be extended)
            var result = await _categoryService.GetAllCategoriesAsync(filter);
            return View(result);
        }
    }
}
