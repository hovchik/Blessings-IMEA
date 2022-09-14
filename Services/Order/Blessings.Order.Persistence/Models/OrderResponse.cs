using Blessings.Shared;

namespace Blessings.OrdersApi.Models;

public class OrderResponse
{
    public int UserId { get; set; }
    public double Price { get; set; }
    public string? Name { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }

    public int SetId { get; set; }
}