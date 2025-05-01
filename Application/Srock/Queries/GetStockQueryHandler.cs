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

            var query = _context.Stock
                .Include(s => s.ProductOption)
                    .ThenInclude(po => po.Product)
                .AsQueryable();

            if (filter.ProductId.HasValue)
                query = query.Where(s => s.ProductOption.ProductId == filter.ProductId.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(s => s.ProductOption.Product.ProductCategoryId == filter.CategoryId.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchKeyword))
                query = query.Where(s => s.ProductOption.Product.Title.Contains(filter.SearchKeyword));

            if (!string.IsNullOrWhiteSpace(request.Filter.SortBy))
            {
                switch (request.Filter.SortBy)
                {
                    case "ProductName":
                        query = request.Filter.SortDescending
                            ? query.OrderByDescending(s => s.ProductOption.Product.Title)
                            : query.OrderBy(s => s.ProductOption.Product.Title);
                        break;
                    case "Available":
                        query = request.Filter.SortDescending
                            ? query.OrderByDescending(s => s.Quantity)
                            : query.OrderBy(s => s.Quantity);
                        break;
                    case "Reserved":
                        query = request.Filter.SortDescending
                            ? query.OrderByDescending(s => s.Reserved)
                            : query.OrderBy(s => s.Reserved);
                        break;
                }
            }

            var resultQuery = query.Select(s => new GetStockDto
            {
                ProductOptionId = s.ProductOptionId,
                ProductTitle = s.ProductOption.Product.Title,
                Size = s.ProductOption.Size,
                AvailableQuantity = s.Quantity,
                ReservedQuantity = s.Reserved
            });

            return await PaginatedList<GetStockDto>.CreateAsync(resultQuery, filter.PageNumber, filter.PageSize);
        }
    }
}
