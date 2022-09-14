using MediatR;

namespace Blessings.Jeweller.Core.CQRS;

public record CreateJewellerCommand : IRequest<CreateJewellerResponse>
{
    public string? Name { get; set; }
    public bool IsAvailable { get; set; }
}