using Application.Category.Queries;
using Application.Common.Models;
using Application.DTOs.Category;
using Application.Interfaces;
using MediatR;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMediator _mediator;

        public CategoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<PaginatedList<ProductCategoryDto>> GetAllCategoriesAsync(CategoryFilterDto filter)
        {
            var query = new GetProductCategoriesQuery(filter);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
