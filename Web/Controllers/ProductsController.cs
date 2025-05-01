using Application.DTOs.Product;
using Application.Products.Commands.AddProduct;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Products.Queries.GetProductById;
using Application.Common.Interfaces;
using Application.Products.Queries.GetProducts;
namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;

        public ProductsController(IMediator mediator, IFileService fileService)
        {
            _mediator = mediator;
            _fileService = fileService;
        }

        [HttpPost("Add-Product")]
        [Authorize(Roles = Roles.InventoryManager)]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDto addProductDto)
        {

            var productId = await _mediator.Send(new AddProductCommand(addProductDto));

            return CreatedAtAction(nameof(GetProductById), new { id = productId }, addProductDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var productDto = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(productDto);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilterDto filter)
        {
            var result = await _mediator.Send(new ProductsQuery(filter));
            return Ok(result);
        }
    }
}
