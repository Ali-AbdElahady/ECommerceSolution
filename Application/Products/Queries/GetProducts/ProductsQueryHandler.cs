using Application.Common.Interfaces;
using Application.DTOs.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.GetProducts
{
    public class ProductsQuery : IRequest<List<ProductDto>>
    {
        public ProductFilterDto Filter { get; set; }

        public ProductsQuery(ProductFilterDto filter)
        {
            Filter = filter;
        }
    }
    public class ProductsQueryHandler  : IRequestHandler<ProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public ProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductDto>> Handle(ProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .Include(p => p.ProductCategory)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(request.Filter.Title))
            {
                query = query.Where(p => p.Title.Contains(request.Filter.Title));
            }

            if (request.Filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.ProductCategoryId == request.Filter.CategoryId.Value);
            }

            // Sorting
            switch (request.Filter.SortBy?.ToLower())
            {
                case "title":
                    query = request.Filter.SortDescending
                        ? query.OrderByDescending(p => p.Title)
                        : query.OrderBy(p => p.Title);
                    break;

                case "category":
                    query = request.Filter.SortDescending
                        ? query.OrderByDescending(p => p.ProductCategory.Name)
                        : query.OrderBy(p => p.ProductCategory.Name);
                    break;

                default:
                    query = query.OrderBy(p => p.Title);
                    break;
            }

            // Pagination
            query = query
                .Skip((request.Filter.PageNumber - 1) * request.Filter.PageSize)
                .Take(request.Filter.PageSize);

            return await query
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CategoryName = p.ProductCategory.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
