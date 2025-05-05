using Application.DTOs.Order;
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
        private readonly IOrderService _orderService;

        public CartController(IProductService productService,IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
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
        public async Task<IActionResult> Checkout([FromBody] List<CreateOrderItemDto> items)
        {
            if (!User.Identity.IsAuthenticated)
            {
                //return Unauthorized(new { redirectUrl = Url.Page("/Account/Login", new { area = "Identity", returnUrl = "/Client/Cart" }) });
                return Unauthorized(new
                {
                    redirectUrl = Url.Action("Login", "Account", new { area = "Identity", returnUrl = "/Client/Cart" })
                });

            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var orderDto = new CreateOrderDto
            {
                CustomerId = userId,
                Items = items
            };

            // Call your order service
            await _orderService.CreateOrderAsync(orderDto);

            return Ok(new { redirectUrl = Url.Action("ThankYou", "Cart", new { area = "Client" }) });
        }

        [HttpGet]
        [Route("ThankYou")]
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
