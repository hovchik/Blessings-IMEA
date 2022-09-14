using FluentValidation;

namespace Blessings.Jeweller.Core.CQRS;

public class CreateJewellerValidation : AbstractValidator<CreateJewellerCommand>
{
    public CreateJewellerValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Jeweller should have a name");
    }
}