using Application.Common.Interfaces;
using Application.Common.Models;
using Application.DTOs.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.GetProducts
{
    public class ProductsQuery : IRequest<PaginatedList<ProductDto>>
    {
        public ProductFilterDto Filter { get; set; }

        public ProductsQuery(ProductFilterDto filter)
        {
            Filter = filter;
        }
    }

    public class ProductsQueryHandler : IRequestHandler<ProductsQuery, PaginatedList<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public ProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<ProductDto>> Handle(ProductsQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            // Apply filtering directly on the projected query
            var query = _context.Products
                .AsNoTracking()
                .Where(p =>
                    (string.IsNullOrWhiteSpace(filter.Title) || p.Title.Contains(filter.Title)) &&
                    (!filter.CategoryId.HasValue || p.ProductCategoryId == filter.CategoryId.Value)
                )
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ImagePath = p.Images.Select(i => i.ImagePath).ToList(),
                    CategoryName = p.ProductCategory.Name
                });

            // Sorting
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "title" => filter.SortDescending
                        ? query.OrderByDescending(p => p.Title)
                        : query.OrderBy(p => p.Title),

                    "category" => filter.SortDescending
                        ? query.OrderByDescending(p => p.CategoryName)
                        : query.OrderBy(p => p.CategoryName),

                    _ => query.OrderBy(p => p.Id)
                };
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            return await PaginatedList<ProductDto>.CreateAsync(
                query,
                filter.PageNumber,
                filter.PageSize);
        }
    }
}
