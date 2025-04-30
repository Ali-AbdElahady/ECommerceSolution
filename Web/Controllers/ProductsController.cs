using Application.DTOs.Product;
using Application.Products.Commands.AddProduct;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Products.Queries.GetProductById;
using Application.Common.Interfaces;
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
            if (addProductDto.Images == null || addProductDto.Images.Count == 0)
            {
                return BadRequest("No images were uploaded.");
            }

            var imagePaths = new List<string>();

            foreach (var image in addProductDto.Images)
            {
                // Save each image and get the file path
                var imagePath = await _fileService.SaveImageAsync(image);
                imagePaths.Add(imagePath);
            }

            // Update the DTO with the saved image paths
            addProductDto.Images = imagePaths;

            // Now proceed to send the DTO to a command or service
            var productId = await _mediator.Send(new AddProductCommand(addProductDto));

            return CreatedAtAction(nameof(GetProductById), new { id = productId }, addProductDto);
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
