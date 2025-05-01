using FluentValidation;

namespace Application.Srock.Commands.AddStock
{
    public class AddStockCommandValidator : AbstractValidator<AddStockCommand>
    {
        public AddStockCommandValidator()
        {
            RuleFor(x => x.addStockDto.ProductOptionId)
                .GreaterThan(0).WithMessage("ProductOptionId must be greater than 0.");

            RuleFor(x => x.addStockDto.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
