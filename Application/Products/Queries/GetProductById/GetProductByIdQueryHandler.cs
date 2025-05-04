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
            var product = await _context.Products
                .Include(p => p.Options).Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ProductCategoryId = product.ProductCategoryId,
                ImagePath = product.Images.Select(i => i.ImagePath).ToList(),
                Options = product.Options.Select(option => new ProductOptionDto
                {
                    Size = option.Size,
                    Price = option.Price
                }).ToList()
            };

            return productDto;
        }
    }
}
