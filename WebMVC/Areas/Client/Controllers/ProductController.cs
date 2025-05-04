using Application.DTOs.Category;
using Application.DTOs.Product;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Areas.Client.Models;

namespace WebMVC.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("Client/Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(ProductFilterDto param)
        {
            var prosParms = new ProductFilterDto
            {
                PageSize = param.PageSize,
                SortBy = "Name",
                PageNumber = param.PageNumber,
                CategoryId = param.CategoryId,
            };

            var catsParms = new CategoryFilterDto
            {
                PageSize = 60,
                SortBy = "Name",
            };

            var featuredProducts = await _productService.GetFeaturedProductsAsync(prosParms);
            var categories = await _categoryService.GetAllCategoriesAsync(catsParms);

            var viewModel = new ProductViewModel
            {
                Products = featuredProducts.Items,
                Categories = categories.Items,
                CategoryId = param.CategoryId,
                PageSize = param.PageSize,
                PageNumber = param.PageNumber,
                Count = featuredProducts.TotalCount,
            };
            return View(viewModel);
        }
        [Route("Details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            var viewModel = new ProductDetailsViewModel
            {
              Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                CategoryName = product.CategoryName,
                ImagePath = product.ImagePath,
                ProductCategoryId = product.ProductCategoryId,
                Options = product.Options,
            };
            

            return View(viewModel);
        }
    }
}
