using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Commands
{
    public class ConfirmShippingCommandValidator : AbstractValidator<ConfirmShippingCommand>
    {
        public ConfirmShippingCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.");
        }
    }
}
