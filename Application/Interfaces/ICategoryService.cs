using Application.Common.Models;
using Application.DTOs.Category;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedList<ProductCategoryDto>> GetAllCategoriesAsync(CategoryFilterDto filter);
    }
}
