using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Stock;
using Domain.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Srock.Commands.ReserveStock
{
    public record ReserveStockCommand(ReserveStockDto reserveStockDto) : IRequest<bool>;
    public class ReserveStockCommandHandler
    {
        private readonly IApplicationDbContext _context;

        public ReserveStockCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
        {
            var productId = request.reserveStockDto.ProductOptionId;
            var quantityToReserve = request.reserveStockDto.Quantity;

            var productOption = await _context.ProductOptions
                .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

            if (productOption == null || productOption.Stock.Quantity < quantityToReserve)
                return false;

            productOption.Stock.Quantity -= quantityToReserve;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
