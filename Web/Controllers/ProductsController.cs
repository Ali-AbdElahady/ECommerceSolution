using Application.DTOs.Product;
using Application.Products.Commands.AddProduct;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Products.Queries.GetProductById;
namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Add-Product")]
        [Authorize(Roles = Roles.InventoryManager)]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto dto)
        {
            var productId = await _mediator.Send(new AddProductCommand(dto));
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.InventoryManager)]
        public async Task<IActionResult> GetProductById(int id)
        {
            var productDto = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(productDto);
        }
    }
}
