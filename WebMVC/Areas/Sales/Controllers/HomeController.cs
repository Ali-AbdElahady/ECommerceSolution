using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Areas.Sales.Models;

namespace WebMVC.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Authorize(Roles = "SalesManager")]
    [Route("Sales/Home")]
    public class HomeController : Controller
    {
        private readonly IOrderService _orderService;

        public HomeController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            var viewModel = orders.Select(order => new OrderListViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                IsShipped = order.IsShipped,
                Items = order.Items.Select(i => new OrderItemViewModel
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            }).ToList();

            return View(viewModel);
        }
    }
}
