using Application.Category.Commands;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Add-ProductCategory")]
        [Authorize(Roles = Roles.InventoryManager)]
        public async Task<IActionResult> AddProductCategory([FromBody] AddProductCategoryCommand command)
        {
            var categoryId = await _mediator.Send(command);
            return CreatedAtAction(nameof(AddProductCategory), new { CategoryId = categoryId });

        }
    }
}
