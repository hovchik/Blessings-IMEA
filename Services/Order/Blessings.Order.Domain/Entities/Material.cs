namespace Blessings.Domain;

public class Material : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Set Set { get; set; }
}