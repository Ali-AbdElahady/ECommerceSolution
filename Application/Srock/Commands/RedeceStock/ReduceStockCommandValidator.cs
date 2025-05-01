using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Srock.Commands.RedeceStock
{
    public class ReduceStockCommandValidator : AbstractValidator<ReduceStockCommand>
    {
        public ReduceStockCommandValidator()
        {
            RuleFor(x => x.reduceStockDto.ProductOptionId)
                .GreaterThan(0).WithMessage("ProductOptionId must be greater than 0.");

            RuleFor(x => x.reduceStockDto.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
