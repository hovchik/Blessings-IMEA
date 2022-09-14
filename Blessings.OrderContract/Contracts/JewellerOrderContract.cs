using Blessings.Shared;

namespace Blessings.Contract;

public class JewellerOrderContract
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}