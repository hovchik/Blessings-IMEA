namespace Blessings.User.Api.Domain;

public class User: EntityBase
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public byte[] Password { get; set; }
    public int Iteration { get; set; }
    public byte[] Salting { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }
}