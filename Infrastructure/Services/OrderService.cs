using Application.DTOs.Order;
using Application.Interfaces;
using Application.Order.Queries;
using Application.Orders.Commands.CreateOrder;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMediator _mediator;

        public OrderService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task CreateOrderAsync(CreateOrderDto orderDto)
        {
            var command = new CreateOrderCommand
            {
                CustomerId = orderDto.CustomerId,
                Items = orderDto.Items
            };

            await _mediator.Send(command);
        }
        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            return await _mediator.Send(new GetAllOrdersQuery());
        }
    }
}
