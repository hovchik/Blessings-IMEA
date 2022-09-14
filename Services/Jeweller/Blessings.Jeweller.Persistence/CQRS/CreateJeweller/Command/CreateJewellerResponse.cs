namespace Blessings.Jeweller.Core.CQRS;

public record CreateJewellerResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsAvailable { get; set; }
}