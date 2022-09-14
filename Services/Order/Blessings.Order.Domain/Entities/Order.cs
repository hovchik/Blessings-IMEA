using Blessings.Shared;

namespace Blessings.Domain;

public class Order : EntityBase
{
    public int UserId { get; set; }
    public double Price { get; set; }
    public string? Name { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }

    public int SetId { get; set; }
    public Set Set { get; set; }
}