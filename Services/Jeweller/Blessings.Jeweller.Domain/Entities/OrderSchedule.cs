namespace Blessings.Jeweller.Domain;

public class OrderSchedule : EntityBase
{
    public int OrderId { get; set; }
    public int MaterialId { get; set; }
    public DateTime CreatedDate { get; set; }
}