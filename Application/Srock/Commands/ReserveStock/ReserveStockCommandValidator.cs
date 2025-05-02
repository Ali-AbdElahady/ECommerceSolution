using FluentValidation;

namespace Application.Srock.Commands.ReserveStock
{
    public class ReserveStockCommandValidator : AbstractValidator<ReserveStockCommand>
    {
        public ReserveStockCommandValidator()
        {
            RuleFor(x => x.reserveStockDtos)
               .NotEmpty().WithMessage("At least one item must be provided.");

            RuleForEach(x => x.reserveStockDtos).ChildRules(dto =>
            {
                dto.RuleFor(d => d.ProductOptionId)
                    .GreaterThan(0).WithMessage("ProductOptionId must be greater than 0.");

                dto.RuleFor(d => d.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            });
        }
    }
}
