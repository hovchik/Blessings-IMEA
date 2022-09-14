namespace Blessings.Jeweller.Domain;

public class Material : EntityBase
{
    public string Name { get; set; }

    public int EstimatedDay { get; set; }

    public string Description { get; set; }

    public ICollection<OrderProcessing> OrderProcessings { get; set; } = new List<OrderProcessing>();
}