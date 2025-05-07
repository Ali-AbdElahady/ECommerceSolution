using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
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
                CustomerId = order.Customer.Id,
                CustomerEmail = order.Customer.Email,
                CustomerPhoneNumber = order.Customer.PhoneNumber,
                CustomerUserName = order.Customer.UserName,
                OrderDate = order.OrderDate,
                IsShipped = order.IsShipped,
                Items = order.Items.Select(i => new OrderItemViewModel
                {
                    ProductId = i.ProductId,
                    ProductTitle = i.ProductTitle,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            }).ToList();

            return View(viewModel);
        }

        [HttpPost("ConfirmOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            await _orderService.ConfirmOrderAsync(orderId);
            return Ok(); 
        }

        [HttpGet]
        [Route("OrderReport/{orderId}")]
        public IActionResult OrderReport(int orderId)
        {
            // Load the order DTO from DB or service
            var order = _orderService.GetOrderReportById(orderId);

            // Create a new report instance
            var report = new StiReport();

            // Load the report template
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "SalesOrder.mrt");
            report.Load(reportPath);

            // Register the business object
            report.RegBusinessObject("Order", order);

            // Compile and render the report
            report.Compile();
            report.Render();

            // Return the report result for the Stimulsoft viewer
            return StiNetCoreViewer.GetReportResult(this, report);
            //return Ok();
        }

    }

}
