#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool IsBlocked { get; set; }
        public float Rating { get; set; }
        public string AvatarUrl { get; set; }
        public int UserId { get; set; }
    }
}
