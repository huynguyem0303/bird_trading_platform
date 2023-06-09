#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsRemoved { get; set; }
        //
        public Category Category { get; set; }
        public Shop Shop { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public IEnumerable<CartDetail> CartDetails { get; set; }
    }
}
