#nullable disable warnings

namespace BirdTrading.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipperCode { get; set; } = null;
        public string CompanyName { get; set; } = null;
        public string ShipperPhone { get; set; } = null;
        //
        public int UserId { get; set; }
        public int? AddressId { get; set; } = null;
        //
        public User User { get; set; }
        public ShippingInformation ShippingInformation { get; set; }
        public IEnumerable<ShippingSession> ShippingSessions { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
