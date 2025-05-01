using Application.DTOs.Stock;
using FluentValidation;

namespace Application.Srock.Queries
{
    public class GetStockFilterDtoValidator : AbstractValidator<GetStockFilterDto>
    {
        public GetStockFilterDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SortBy)
           .Must(value => string.IsNullOrEmpty(value) ||
               new[] { "ProductName", "AvailableQuantity", "ReservedQuantity" }
               .Contains(value))
           .WithMessage("SortBy must be one of: ProductName, AvailableQuantity, ReservedQuantity.");
        }
    }
}
