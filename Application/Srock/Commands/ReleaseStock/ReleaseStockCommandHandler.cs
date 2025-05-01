using Application.Common.Interfaces;
using Application.DTOs.Stock;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Srock.Commands.ReleaseStock
{
    public record ReleaseStockCommand(ReleaseStockDto releaseStockDto) : IRequest<bool>;
    public class ReleaseStockCommandHandler : IRequestHandler<ReleaseStockCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public ReleaseStockCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ReleaseStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stock
                .FirstOrDefaultAsync(s => s.ProductOptionId == request.releaseStockDto.ProductOptionId, cancellationToken);

            if (stock == null || stock.Reserved < request.releaseStockDto.Quantity)
                return false;

            stock.Reserved -= request.releaseStockDto.Quantity;
            stock.Quantity += request.releaseStockDto.Quantity;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
