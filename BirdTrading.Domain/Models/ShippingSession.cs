using BirdTrading.Domain.Enums;

#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class ShippingSession
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime SessionDate { get; set; }
        public string? Description { get; set; }
        //
        public int OrderId { get; set; }
        //
        public Order Order { get; set; }
    }
}
