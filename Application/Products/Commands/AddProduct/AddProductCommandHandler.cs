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
                    Price = option.Price,
                    Stock = new Stock { Quantity = option.StockQuantity }
                }).ToList()
            };


            // Save images and create Image entities
            if (request.Product.Images != null && request.Product.Images.Any())
            {
                foreach (var imageFile in request.Product.Images)
                {
                    var imagePath = await _fileService.SaveImageAsync(imageFile);
                    product.Images.Add(new Image
                    {
                        ImagePath = imagePath
                    });
                }
            }


            //var imagePaths = new List<string>();

            //foreach (var image in request.Product.Images)
            //{
            //    var imagePath = await _fileService.SaveImageAsync(image);
            //}

            _context.Products.Add(product);

            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
