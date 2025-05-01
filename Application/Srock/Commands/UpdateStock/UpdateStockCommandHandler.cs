using Application.Common.Interfaces;
using Application.DTOs.Stock;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Srock.Commands.UpdateStock
{
    public record UpdateStockCommand(UpdateStockDto updateStockDto) : IRequest<bool>;
    public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateStockCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var dto = request.updateStockDto;

            var productOption = await _context.ProductOptions
                .FirstOrDefaultAsync(po => po.Id == dto.ProductOptionId, cancellationToken);

            if (productOption == null)
                return false;

            productOption.Stock.Quantity = dto.NewQuantity;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
