#nullable disable warnings
using System.ComponentModel.DataAnnotations;

namespace BirdTrading.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal OriginalPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsRemoved { get; set; }
        //
        public int CategoryId { get; set; }
        public int ShopId { get; set; }
        //
        public Category Category { get; set; }
        public Shop Shop { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public IEnumerable<CartDetail> CartDetails { get; set; }
    }
}
