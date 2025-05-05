using Application.Orders.Commands.CreateOrder;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.")
            .Must(id => !string.IsNullOrWhiteSpace(id)).WithMessage("CustomerId must be a valid non-empty string.");

            RuleFor(x => x.Items).NotEmpty().WithMessage("At least one order item is required.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).GreaterThan(0);
                item.RuleFor(i => i.OptionId).GreaterThan(0);
                item.RuleFor(i => i.Quantity).GreaterThan(0);
            });
        }
    }
}
