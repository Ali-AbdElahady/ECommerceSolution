using Application.Common.Models;
using Application.DTOs.Product;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedList<ProductDto>> GetFeaturedProductsAsync(ProductFilterDto filter);
    }
}
