namespace Blessings.User.Api.Domain;

public class Role: EntityBase
{
    public string Name { get; set; }
    public int Type { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}