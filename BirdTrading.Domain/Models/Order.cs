#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsPaid { get; set; }
        public string ShipperCode { get; set; }
        public string CompanyName { get; set; }
        public string ShipperPhone { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
    }
}
