using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Category.Commands
{
    public class AddProductCategoryCommandValidator : AbstractValidator<AddProductCategoryCommand>
    {
        public AddProductCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        }
    }
}
