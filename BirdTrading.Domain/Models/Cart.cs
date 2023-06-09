#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        //
        public int UserId { get; set; }
        public int ShopId { get; set; }
        //
        public Shop Shop { get; set; }
        public User User { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }
    }
}