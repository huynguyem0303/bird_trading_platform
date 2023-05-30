#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class ShippingInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public bool isDefaultAddress { get; set; }
    }
}
