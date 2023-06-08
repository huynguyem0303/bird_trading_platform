#nullable disable warnings
namespace BirdTrading.Domain.Models
{
    public class CategoryType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string ImageURL { get; set; }
        //
        public IEnumerable<Category> Categories { get; set; }
    }
}
