using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Areas.Client.Models;

namespace WebMVC.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("Client/Cart")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new List<CartItemViewModel>()); // Empty view, populated by JS
        }

        [HttpPost]
        [Route("Load")]
        public async Task<IActionResult> Load([FromBody] List<CartItemDto> cartItems)
        {
            var viewModel = new List<CartItemViewModel>();

            foreach (var item in cartItems)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                var option = product?.Options?.FirstOrDefault(o => o.Id == item.OptionId);

                if (product != null && option != null)
                {
                    viewModel.Add(new CartItemViewModel
                    {
                        ProductId = item.ProductId,
                        Title = product.Title,
                        Size = option.Size,
                        Price = option.Price,
                        Quantity = item.Quantity, 
                        OptionId = option.Id
                    });
                }
            }

            return PartialView("_CartTable", viewModel); // render partial HTML and return
        }

        [HttpPost]
        [Route("Checkout")]
        public IActionResult Checkout([FromBody] List<CartItemDto> cartItems)
        {
            // Save order to DB or process checkout
            return Ok(new { success = true, message = "Order placed successfully." });
        }
    }
}
