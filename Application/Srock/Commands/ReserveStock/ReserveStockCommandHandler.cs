using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Stock;
using Domain.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Srock.Commands.ReserveStock
{
    public record ReserveStockCommand(List<ReserveStockDto> reserveStockDtos) : IRequest<bool>;
    public class ReserveStockCommandHandler
    {
        private readonly IApplicationDbContext _context;

        public ReserveStockCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
        {
            foreach (var dto in request.reserveStockDtos)
            {
                var option = await _context.ProductOptions
                    .Include(po => po.Stock)
                    .FirstOrDefaultAsync(po => po.Id == dto.ProductOptionId, cancellationToken);

                if (option == null || option.Stock == null || option.Stock.Quantity < dto.Quantity)
                    return false;

                option.Stock.Quantity -= dto.Quantity;
                option.Stock.Reserved += dto.Quantity;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
