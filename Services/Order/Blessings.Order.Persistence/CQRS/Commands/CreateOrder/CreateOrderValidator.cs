using FluentValidation;

namespace Blessings.Order.Core.CQRS.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(p => p.UserId)
            .NotEqual(0)
            .NotNull();
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{Name} is required.")
            .NotNull()
            .MaximumLength(100).WithMessage("{Name} must not exceed 100 characters.");

        RuleFor(p => p.SetId)
            .NotEqual(0).NotNull()
            .WithMessage("{SetId} is required.");
    }
}