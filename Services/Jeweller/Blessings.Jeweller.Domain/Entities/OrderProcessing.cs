using Blessings.Shared;

namespace Blessings.Jeweller.Domain;

public class OrderProcessing : EntityBase
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }

    public int JewellerId { get; set; }
    public Jeweller Jeweller { get; set; }

    public int MaterialId { get; set; }
    public Material Material { get; set; }

}