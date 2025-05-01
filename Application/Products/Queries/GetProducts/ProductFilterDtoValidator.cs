using Application.DTOs.Product;
using FluentValidation;

namespace Application.Products.Queries.GetProducts
{
    public class ProductFilterDtoValidator : AbstractValidator<ProductFilterDto>
    {
        public ProductFilterDtoValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

            RuleFor(x => x.SortBy)
                .Must(sort => string.IsNullOrEmpty(sort) ||
                    new[] { "title", "category" }.Contains(sort.ToLower()))
                .WithMessage("SortBy must be either 'title' or 'category'.");
        }
    }
}
