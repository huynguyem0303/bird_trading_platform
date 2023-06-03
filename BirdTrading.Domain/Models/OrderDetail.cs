#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public float? Rating { get; set; } = null;
        //
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        //
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
