using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.DTOs.Stock;
using Application.Srock.Commands.RedeceStock;

namespace Application.Order.Commands
{
    public class ConfirmShippingCommandHandler : IRequestHandler<ConfirmShippingCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        private readonly IIdentityService _identityService;

        public ConfirmShippingCommandHandler(IApplicationDbContext context,
            IMediator mediator,
            INotificationService notificationService,IIdentityService identityService)
        {
            _context = context;
            _mediator = mediator;
            _notificationService = notificationService;
            _identityService = identityService;
        }
        public async Task<bool> Handle(ConfirmShippingCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null || order.IsShipped)
            {
                return false;
            }

            foreach (var item in order.OrderItems)
            {
                var reduceDto = new ReduceStockDto
                {
                    ProductOptionId = item.ProductOptionId,
                    Quantity = item.Quantity
                };

                var result = await _mediator.Send(new ReduceStockCommand(reduceDto), cancellationToken);

                if (!result)
                    throw new Exception($"Failed to reduce stock for ProductOptionId: {item.ProductOptionId}");
            }

            order.IsShipped = true;
            await _context.SaveChangesAsync(cancellationToken);

            var client = await _identityService.GetUserByIdAsync(order.CustomerId);
            // token of the clinet
            await _notificationService.SendNotificationAsync("New Order", $"Order #{order.Id} placed", client.FCMToken ?? "fake token");

            return true;
        }
    }
}
