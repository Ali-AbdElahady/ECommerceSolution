using Application.DTOs.Order;
using Application.DTOs.Product;
using Application.Order.Commands;
using Application.Order.Queries;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // Only Sales Managers can confirm shipping
        [HttpPost("confirm-shipping")]
        [Authorize(Roles = Roles.SalesManager)]
        public async Task<IActionResult> ConfirmShipping([FromBody] ConfirmShippingCommand order)
        {
            var result = await _mediator.Send(order);

            if (!result)
            {
                return BadRequest(new { Message = "Order not found or already shipped." });
            }

            return Ok(new { Message = "Order shipping confirmed successfully." });
        }

        [HttpGet("orders")]
        [Authorize(Roles = Roles.SalesManager)]
        public async Task<IActionResult> orders()
        {
            // Send the GetAllOrdersQuery to the mediator to get the orders
            var orders = await _mediator.Send(new GetAllOrdersQuery());

            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found.");
            }

            // Return the orders as a response
            return Ok(orders);
        }

        // Only Inventory Managers can add new products
        [HttpPost("add-product")]
        [Authorize(Roles = Roles.InventoryManager)]
        public IActionResult AddProduct([FromBody] ProductDto product)
        {
            // InventoryManager role-specific logic here
            return Ok();
        }

        // Any authenticated client can place an order
        [HttpPost("place-order")]
        [Authorize(Roles = Roles.Client)]
        public IActionResult PlaceOrder([FromBody] OrderDto order)
        {
            // Client logic here
            return Ok();
        }
    }
}
