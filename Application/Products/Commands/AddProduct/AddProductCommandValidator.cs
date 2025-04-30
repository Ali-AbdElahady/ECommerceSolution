using FluentValidation;

namespace Application.Products.Commands.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.Product.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100);

            RuleFor(x => x.Product.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.Product.ProductCategoryId)
                .GreaterThan(0).WithMessage("Product category must be selected.");

            RuleForEach(x => x.Product.Options)
                .ChildRules(option =>
                {
                    option.RuleFor(o => o.Size)
                        .NotEmpty().WithMessage("Option size is required.");

                    option.RuleFor(o => o.Price)
                        .GreaterThan(0).WithMessage("Option price must be greater than zero.");
                });
        }
    }
}
