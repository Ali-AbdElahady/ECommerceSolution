using Application.DTOs.Category;
using Application.DTOs.Product;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Areas.Client.Models;

namespace WebMVC.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("Client/Home")]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var prosParms = new ProductFilterDto
            {
                PageSize = 6,
                SortBy = "Name",
            };

            var catsParms = new CategoryFilterDto
            {
                PageSize = 3,
                SortBy = "Name",
            };

            var featuredProducts = await _productService.GetFeaturedProductsAsync(prosParms);
            var categories = await _categoryService.GetAllCategoriesAsync(catsParms);

            var viewModel = new HomeViewModel
            {
                FeaturedProducts = featuredProducts.Items,
                Categories = categories.Items
            };
            return View(viewModel);
        }
    }
}
