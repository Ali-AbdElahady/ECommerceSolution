using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public AddProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Title = request.Product.Title,
                Description = request.Product.Description,
                ProductCategoryId = request.Product.ProductCategoryId,
                Options = request.Product.Options.Select(option => new ProductOption
                {
                    Size = option.Size,
                    Price = option.Price
                }).ToList()
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
