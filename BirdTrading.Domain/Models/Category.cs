#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        //
        public CategoryType Type { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
