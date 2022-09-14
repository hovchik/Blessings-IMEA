using Blessings.Shared;

namespace Blessings.Contract
{
    public class OrderContract
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int MaterialId { get; set; }
        public DateTime CreatedDate { get; set; }

        public OrderStatus Status { get; set; }

    }
}