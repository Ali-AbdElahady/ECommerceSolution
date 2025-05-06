using Application.DTOs.Category;
using Application.DTOs.Product;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMVC.Areas.Client.Models;
using WebMVC.Areas.Inventory.Models;

namespace WebMVC.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = "InventoryManager")]
    [Route("Inventory/Product")]
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
        [HttpGet("Create")]
        public async Task<IActionResult> Create() 
        {
            var catsParms = new CategoryFilterDto
            {
                PageSize = 60,
                SortBy = "Name",
            };
            var categories = await _categoryService.GetAllCategoriesAsync(catsParms);
            var model = new ProductCreateViewModel
            {
                Categories = categories.Items.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).Prepend(new SelectListItem { Value = "", Text = "-- Select Category --" })
                .ToList()
            };
            return View(model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate category list on validation fail
                var catsParms = new CategoryFilterDto
                {
                    PageSize = 60,
                    SortBy = "Name",
                };
                var categories = await _categoryService.GetAllCategoriesAsync(catsParms);

                model.Categories = categories.Items.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).Prepend(new SelectListItem { Value = "", Text = "-- Select Category --" })
                .ToList();

                return View(model);
            }

            // Prepare DTO
            var addProductDto = new AddProductDto
            {
                Title = model.Title,
                Description = model.Description,
                ProductCategoryId = model.CategoryId,
                Images = model.Images.ToList(),
                Options = model.Options.Select(opt => new ProductOptionDto
                {
                    Size = opt.Size,
                    Price = opt.Price,
                    StockQuantity = opt.StockQuantity
                }).ToList()
            };

            await _productService.CreateProductAsync(addProductDto);

            TempData["Success"] = "Product created successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var categories = await _categoryService.GetAllCategoriesAsync(new CategoryFilterDto());

            var viewModel = new ProductCreateViewModel
            {
                Title = product.Title,
                Description = product.Description,
                CategoryId = product.ProductCategoryId,
                Categories = categories.Items.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == product.ProductCategoryId
                }).ToList(),
                Options = product.Options.Select(o => new ProductOptionInputModel
                {
                    Size = o.Size,
                    Price = o.Price,
                    StockQuantity = o.StockQuantity
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync(new CategoryFilterDto());
                model.Categories = categories.Items.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == model.CategoryId
                }).ToList();
                return View(model);
            }

            var dto = new AddProductDto
            {
                Title = model.Title,
                Description = model.Description,
                ProductCategoryId = model.CategoryId,
                Images = model.Images,
                Options = model.Options.Select(o => new ProductOptionDto
                {
                    Size = o.Size,
                    Price = o.Price,
                    StockQuantity = o.StockQuantity
                }).ToList()
            };

            await _productService.UpdateProductAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
