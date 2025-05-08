using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Category.Commands
{
    public class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
    {
        public UpdateProductCategoryCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        }
    }
}
