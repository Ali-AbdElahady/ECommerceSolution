using Application.Common.Models;
using Application.DTOs.Product;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedList<ProductDto>> GetFeaturedProductsAsync(ProductFilterDto filter);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(AddProductDto productDto);
        Task UpdateProductAsync(int id, AddProductDto viewModel);
    }
}
