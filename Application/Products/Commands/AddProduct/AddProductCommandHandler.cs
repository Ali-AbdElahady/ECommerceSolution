using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;
        public AddProductCommandHandler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
            _fileService = fileService;
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var imagePaths = new List<string>();

            foreach (var image in request.Product.Images)
            {
                var imagePath = await _fileService.SaveImageAsync(image);
                imagePaths.Add(imagePath);
            }

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
