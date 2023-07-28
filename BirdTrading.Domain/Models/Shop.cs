#nullable disable warnings
using System.ComponentModel.DataAnnotations;

namespace BirdTrading.Domain.Models
{
    public class Shop
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsBlocked { get; set; }
        public float Rating { get; set; }
        public string AvatarUrl { get; set; }
        //
        public int UserId { get; set; }
        //
        public User User { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
