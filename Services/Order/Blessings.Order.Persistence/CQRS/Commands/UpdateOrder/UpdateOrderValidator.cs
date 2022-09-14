using FluentValidation;

namespace Blessings.Order.Core.CQRS.Commands.UpdateOrder;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(p => p.Status).NotEmpty();
    }
}