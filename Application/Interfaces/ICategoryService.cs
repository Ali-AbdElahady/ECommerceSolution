using Application.Common.Models;
using Application.DTOs.Category;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedList<ProductCategoryDto>> GetAllCategoriesAsync(CategoryFilterDto filter);
    }
}
