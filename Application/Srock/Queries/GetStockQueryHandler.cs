using Application.Common.Interfaces;
using Application.Common.Models;
using Application.DTOs.Stock;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Srock.Queries
{
    public class GetStockQuery : IRequest<PaginatedList<GetStockDto>>
    {
        public GetStockFilterDto Filter { get; set; }

        public GetStockQuery(GetStockFilterDto filter)
        {
            Filter = filter;
        }
    }

    public class GetStockQueryHandler : IRequestHandler<GetStockQuery, PaginatedList<GetStockDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetStockQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<GetStockDto>> Handle(GetStockQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            // Projection is now applied immediately
            var query = _context.Stock
                .AsNoTracking()
                .Where(s =>
                    (!filter.ProductId.HasValue || s.ProductOption.ProductId == filter.ProductId.Value) &&
                    (!filter.CategoryId.HasValue || s.ProductOption.Product.ProductCategoryId == filter.CategoryId.Value) &&
                    (string.IsNullOrWhiteSpace(filter.SearchKeyword) || s.ProductOption.Product.Title.Contains(filter.SearchKeyword))
                )
                .Select(s => new GetStockDto
                {
                    ProductOptionId = s.ProductOptionId,
                    ProductTitle = s.ProductOption.Product.Title,
                    Size = s.ProductOption.Size,
                    AvailableQuantity = s.Quantity,
                    ReservedQuantity = s.Reserved
                });

            // Sorting
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.SortBy switch
                {
                    "ProductName" => filter.SortDescending
                        ? query.OrderByDescending(s => s.ProductTitle)
                        : query.OrderBy(s => s.ProductTitle),

                    "Available" => filter.SortDescending
                        ? query.OrderByDescending(s => s.AvailableQuantity)
                        : query.OrderBy(s => s.AvailableQuantity),

                    "Reserved" => filter.SortDescending
                        ? query.OrderByDescending(s => s.ReservedQuantity)
                        : query.OrderBy(s => s.ReservedQuantity),

                    _ => query
                };
            }

            return await PaginatedList<GetStockDto>.CreateAsync(query, filter.PageNumber, filter.PageSize);
        }
    }
}
