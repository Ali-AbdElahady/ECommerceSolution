using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Application.Order.Commands
{
    public class ConfirmShippingCommandHandler : IRequestHandler<ConfirmShippingCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        public ConfirmShippingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
         public async Task<bool> Handle(ConfirmShippingCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null || order.IsShipped)
            {
                return false;
            }

            order.IsShipped = true;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
