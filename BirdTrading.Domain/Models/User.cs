using BirdTrading.Domain.Enums;

#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string AvatarURL { get; set; }
        public bool IsTempUser { get; set; }
        public bool IsBlocked { get; set; }
        //
        public int ShippingInforId { get; set; }
        //
        public IEnumerable<ShippingInformation> ShippingInformations { get; set; }
        public Shop Shop { get; set; } = null;
        public IEnumerable<Order> Orders { get; set; }
    }
}
