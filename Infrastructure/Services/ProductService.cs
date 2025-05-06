using Application.Common.Models;
using Application.DTOs.Product;
using Application.Interfaces;
using Application.Products.Commands.AddProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries.GetProductById;
using Application.Products.Queries.GetProducts;
using MediatR;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IMediator _mediator;

        public ProductService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<PaginatedList<ProductDto>> GetFeaturedProductsAsync(ProductFilterDto filter)
        {
            var query = new ProductsQuery(filter);
            var result = await _mediator.Send(query);
            return result;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var query = new GetProductByIdQuery(id); 
            return await _mediator.Send(query);
        }
        public async Task<int> CreateProductAsync(AddProductDto productDto)
        {
            var command = new AddProductCommand(productDto);
            var productId = await _mediator.Send(command);
            return productId;
        }

        public async Task UpdateProductAsync(int id, AddProductDto viewModel)
        {
            var command = new UpdateProductCommand
            {
                Id = id,
                Product = viewModel
            };

            await _mediator.Send(command);
        }
    }
}
