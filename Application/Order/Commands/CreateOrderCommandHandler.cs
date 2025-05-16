using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Order;
using Application.DTOs.Stock;
using Application.Srock.Commands.ReserveStock;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderEntity = Domain.Entities.Order;

namespace Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<int>
    {
        public string CustomerId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        private readonly IIdentityService _identityService;

        public CreateOrderCommandHandler(
            IApplicationDbContext context,
            IMediator mediator,
            INotificationService notificationService,
            IIdentityService identityService)
        {
            _context = context;
            _mediator = mediator;
            _notificationService = notificationService;
            _identityService = identityService;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new OrderEntity
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                IsShipped = false,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in request.Items)
            {
                var option = await _context.ProductOptions
                    .FirstOrDefaultAsync(o => o.Id == item.OptionId && o.ProductId == item.ProductId, cancellationToken);

                if (option == null)
                    throw new NotFoundException($"Product option not found for productId={item.ProductId}", item.OptionId);

                order.OrderItems.Add(new OrderItem
                {
                    ProductOptionId = item.OptionId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = option.Price
                });
            }

            var reserveDtos = order.OrderItems.Select(oi => new ReserveStockDto
            {
                ProductOptionId = oi.ProductOptionId,
                Quantity = oi.Quantity,
                OrderId = order.Id
            }).ToList();

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);

                var reserveResult = await _mediator.Send(new ReserveStockCommand(reserveDtos), cancellationToken);

                if (!reserveResult)
                    throw new Exception("Unable to reserve stock for one or more items.");

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            var salesManager = await _identityService.GetUserByEmailAsync("ali.test.292100@gmail.com");
            var fcmToken = salesManager.FCMToken ?? "salesManager not logged yet";
            await _notificationService.SendNotificationAsync("New Order", $"Order #{order.Id} placed", fcmToken);

            return order.Id;
        }
    }
}
