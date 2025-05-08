using Application.DTOs.Category;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = "InventoryManager")]
    [Route("Inventory/Category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var filter = new CategoryFilterDto(); 
            var result = await _categoryService.GetAllCategoriesAsync(filter);
            return View(result);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return View(categoryDto);

            var success = await _categoryService.UpdateCategoryAsync(categoryDto);
            if (!success)
            {
                ModelState.AddModelError("", "Update failed.");
                return View(categoryDto);
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _categoryService.AddCategoryAsync(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}
