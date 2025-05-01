using Application.Common.Interfaces;
using Application.DTOs.Stock;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Srock.Commands.RedeceStock
{
    public record ReduceStockCommand(ReduceStockDto reduceStockDto) : IRequest<bool>;
    public class ReduceStockCommandHandler : IRequestHandler<ReduceStockCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public ReduceStockCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ReduceStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stock
                .FirstOrDefaultAsync(s => s.ProductOptionId == request.reduceStockDto.ProductOptionId, cancellationToken);

            if (stock == null || stock.Quantity < request.reduceStockDto.Quantity)
            {
                return false; // Not enough stock to reduce
            }

            stock.Quantity -= request.reduceStockDto.Quantity;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
