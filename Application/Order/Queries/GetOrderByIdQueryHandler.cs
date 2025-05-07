using Application.Common.Interfaces;
using Application.DTOs.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetOrderByIdQueryHandler(IApplicationDbContext context,IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product) // Ensure ProductTitle is accessible
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
                return null;

            var customer = await _identityService.GetUserByIdAsync(order.CustomerId);

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                IsShipped = order.IsShipped,
                Customer = customer == null
                    ? new CustomerDto()
                    : new CustomerDto
                    {
                        Id = customer.Id,
                        Email = customer.Email,
                        UserName = customer.UserName,
                        PhoneNumber = customer.PhoneNumber
                    },
                Items = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductOptionId = item.ProductOptionId,
                    ProductTitle = item.Product?.Title ?? "N/A",
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
        }
    }
}
