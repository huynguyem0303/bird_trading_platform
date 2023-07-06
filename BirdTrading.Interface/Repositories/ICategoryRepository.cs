using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryNameAysnc(int id);
        Task<IEnumerable<Category>> GetListByTypeIdAsync(int typeId);
    }
}
