//using Application.Common.Interfaces;
//using Application.DTOs.Order;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;

//namespace Application.Order.Queries
//{
//    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
//    {
//        private readonly IApplicationDbContext _context;
//        private readonly IIdentityService _identityService;

//        public GetAllOrdersQueryHandler(IApplicationDbContext context, IIdentityService identityService)
//        {
//            _context = context;
//            _identityService = identityService;
//        }

//        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
//        {
//            var orders = await _context.Orders
//                .Include(o => o.OrderItems)
//                .ThenInclude(oi => oi.Product)
//                .ToListAsync(cancellationToken);

//            var orderDtos = new List<OrderDto>();

//            foreach (var order in orders)
//            {
//                var customer = await _identityService.GetUserByIdAsync(order.CustomerId);

//                var orderDto = new OrderDto
//                {
//                    Id = order.Id,
//                    OrderDate = order.OrderDate,
//                    IsShipped = order.IsShipped,
//                    Customer = new CustomerDto
//                    {
//                        Id = customer.Id,
//                        Email = customer.Email,
//                        UserName = customer.UserName,
//                        PhoneNumber = customer.PhoneNumber
//                    },
//                    Items = order.OrderItems.Select(oi => new OrderItemDto
//                    {
//                        ProductId = oi.ProductId,
//                        ProductTitle = oi.Product.Title,
//                        Quantity = oi.Quantity,
//                        Price = oi.Price
//                    }).ToList()
//                };

//                orderDtos.Add(orderDto);
//            }

//            return orderDtos;
//        }

//    }
//}


using Application.Common.Interfaces;
using Application.DTOs.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Order.Queries
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetAllOrdersQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderData = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.IsShipped,
                    o.CustomerId,
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductTitle = oi.Product.Title,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            var orderDtos = new List<OrderDto>();

            foreach (var order in orderData)
            {
                var customer = await _identityService.GetUserByIdAsync(order.CustomerId);

                orderDtos.Add(new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    IsShipped = order.IsShipped,
                    Items = order.Items,
                    Customer = new CustomerDto
                    {
                        Id = customer.Id,
                        Email = customer.Email,
                        UserName = customer.UserName,
                        PhoneNumber = customer.PhoneNumber
                    }
                });
            }

            return orderDtos;
        }
    }
}
