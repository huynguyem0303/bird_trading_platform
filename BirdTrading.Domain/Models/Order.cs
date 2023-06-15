#nullable disable warnings

namespace BirdTrading.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? ShipperCode { get; set; } 
        public string? CompanyName { get; set; }
        public string? ShipperPhone { get; set; }
        public decimal Total { get; set; }
        //
        public int UserId { get; set; }
        public int? ShippingInformationId { get; set; } = null;
        //
        public User User { get; set; }
        public ShippingInformation ShippingInformation { get; set; }
        public IEnumerable<ShippingSession> ShippingSessions { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
