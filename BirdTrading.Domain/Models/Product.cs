#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal originalPrice { get; set; }
        public decimal discountPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsRemoved { get; set; }
    }
}
