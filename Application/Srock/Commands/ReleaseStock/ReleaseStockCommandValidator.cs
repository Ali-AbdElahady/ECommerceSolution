using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Srock.Commands.ReleaseStock
{
    public class ReleaseStockCommandValidator : AbstractValidator<ReleaseStockCommand>
    {
        public ReleaseStockCommandValidator()
        {
            RuleFor(x => x.releaseStockDto.ProductOptionId)
                .GreaterThan(0).WithMessage("ProductOptionId must be greater than 0.");

            RuleFor(x => x.releaseStockDto.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
