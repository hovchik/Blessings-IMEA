namespace Blessings.Domain;

public class Set : EntityBase
{
    public string? Name { get; set; }

    public int MaterialId { get; set; }
    public Material Material { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();

}