using Application.Common.Interfaces;
using Application.Common.Models;
using Application.DTOs.Category;
using Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Category.Queries
{
    public class GetProductCategoriesQuery : IRequest<PaginatedList<ProductCategoryDto>>
    {
        public CategoryFilterDto Filter { get; set; }

        public GetProductCategoriesQuery(CategoryFilterDto filter)
        {
            Filter = filter;
        }
    }
    public class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, PaginatedList<ProductCategoryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductCategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<ProductCategoryDto>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.ProductCategories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Filter.Name))
                query = query.Where(c => c.Name.Contains(request.Filter.Name));
            if (request.Filter.SortBy?.ToLower() == "name")
                query = request.Filter.SortDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
            else
                query = query.OrderBy(c => c.Id);
            var dtoQuery = query.Select(c => new ProductCategoryDto { Id = c.Id, Name = c.Name });
            return await PaginatedList<ProductCategoryDto>.CreateAsync(dtoQuery, request.Filter.PageNumber, request.Filter.PageSize);
        }
    }
}
