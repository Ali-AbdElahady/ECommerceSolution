using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Product;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<int>
    {
        public int Id { get; set; }
        public AddProductDto Product { get; set; } = new();
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;

        public UpdateProductCommandHandler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.Options)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
                throw new NotFoundException(nameof(Product), request.Id);

            // Update core fields
            product.Title = request.Product.Title;
            product.Description = request.Product.Description;
            product.ProductCategoryId = request.Product.ProductCategoryId;

            // Clear and rebuild image list
            product.Images.Clear();

            // Add preserved images
            if (request.Product.ExistingImagePaths != null)
            {
                foreach (var path in request.Product.ExistingImagePaths)
                {
                    product.Images.Add(new Image { ImagePath = path });
                }
            }

            // Add new uploaded images
            if (request.Product.Images != null)
            {
                foreach (var image in request.Product.Images)
                {
                    var imagePath = await _fileService.SaveImageAsync(image);
                    product.Images.Add(new Image { ImagePath = imagePath });
                }
            }

            // Replace product options
            _context.ProductOptions.RemoveRange(product.Options);
            product.Options = request.Product.Options.Select(o => new ProductOption
            {
                Size = o.Size,
                Price = o.Price,
                Stock = new Stock { Quantity = o.StockQuantity }
            }).ToList();

            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}
