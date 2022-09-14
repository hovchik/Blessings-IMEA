namespace Blessings.Jeweller.Domain;

public class Jeweller : EntityBase
{
    public string? Name { get; set; }

    public bool IsAvailable { get; set; }

    public ICollection<OrderProcessing> OrderProcessing { get; set; } = new List<OrderProcessing>();

}