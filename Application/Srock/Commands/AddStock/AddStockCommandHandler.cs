using Application.Common.Interfaces;
using Application.DTOs.Stock;
using Domain.Entities;
using MediatR;

namespace Application.Srock.Commands.AddStock
{
    public record AddStockCommand(AddStockDto addStockDto) : IRequest<int>;
    public class AddStockCommandHandler : IRequestHandler<AddStockCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public AddStockCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            var stock = new Stock
            {
                ProductOptionId = request.addStockDto.ProductOptionId,
                Quantity = request.addStockDto.Quantity,
                Reserved = 0
            };

            _context.Stock.Add(stock);
            await _context.SaveChangesAsync(cancellationToken);

            return stock.Id;
        }
    }
}
