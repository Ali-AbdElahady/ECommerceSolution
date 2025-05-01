using FluentValidation;

namespace Application.Srock.Commands.UpdateStock
{
    public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockCommandValidator()
        {
            RuleFor(x => x.updateStockDto.ProductOptionId)
                .GreaterThan(0).WithMessage("Product option ID must be greater than zero.");

            RuleFor(x => x.updateStockDto.NewQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be zero or greater.");
        }
    }
}
