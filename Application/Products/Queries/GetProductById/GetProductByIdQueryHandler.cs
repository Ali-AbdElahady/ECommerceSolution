using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Product;
using Application.Products.Queries.GetProductById;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IApplicationDbContext _context;

        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productDto = await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ProductCategoryId = p.ProductCategoryId,
                    ImagePath = p.Images.Select(i => i.ImagePath).ToList(),
                    Options = p.Options.Select(option => new ProductOptionDto
                    {
                        Id = option.Id,
                        Size = option.Size,
                        Price = option.Price,
                        StockQuantity = option.Stock.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (productDto == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            return productDto;
        }
    }
}
