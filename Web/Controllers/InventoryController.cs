using Application.Common.Exceptions;
using Application.DTOs.Stock;
using Application.Srock.Commands.AddStock;
using Application.Srock.Commands.ReserveStock;
using Application.Srock.Commands.UpdateStock;
using Application.Srock.Queries;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize(Roles = Roles.InventoryManager)]
        [HttpPost("add-stock")]
        public async Task<IActionResult> AddStock([FromBody] AddStockDto dto)
        {
            var stockId = await _mediator.Send(new AddStockCommand(dto));
            return CreatedAtAction("GetStockById", new { id = stockId }, dto);
        }

        [HttpPut("update-stock")]
        [Authorize(Roles = Roles.InventoryManager)]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockDto dto)
        {
            var result = await _mediator.Send(new UpdateStockCommand(dto));
            return result ? Ok("Stock updated successfully.") : throw new NotFoundException(nameof(Stock),dto);
        }

        [HttpGet("stock")]
        [Authorize(Roles = Roles.InventoryManager)]
        public async Task<IActionResult> GetStock([FromQuery] GetStockFilterDto filter)
        {
            var query = new GetStockQuery(filter);
            var stockList = await _mediator.Send(query);
            return Ok(stockList);
        }
    }
}
