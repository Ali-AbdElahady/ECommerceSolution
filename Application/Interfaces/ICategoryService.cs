using Application.Category.Commands;
using Application.Common.Models;
using Application.DTOs.Category;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedList<ProductCategoryDto>> GetAllCategoriesAsync(CategoryFilterDto filter);
        Task<bool> UpdateCategoryAsync(ProductCategoryDto dto);
        Task<int> AddCategoryAsync(ProductCategoryDto dto);
    }
}
