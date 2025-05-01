using FluentValidation;

namespace Application.Srock.Commands.ReserveStock
{
    public class ReserveStockCommandValidator : AbstractValidator<ReserveStockCommand>
    {
        public ReserveStockCommandValidator()
        {
            RuleFor(x => x.reserveStockDto.ProductOptionId)
                .GreaterThan(0).WithMessage("ProductOptionId must be greater than 0.");

            RuleFor(x => x.reserveStockDto.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
