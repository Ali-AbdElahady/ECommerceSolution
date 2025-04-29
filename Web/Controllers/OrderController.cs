using Application.DTOs.Order;
using Application.DTOs.Product;
using Application.Order.Queries;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult ConfirmShipping([FromBody] OrderDto order)
        {
            // SalesManager role-specific logic here
            return Ok();
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
