using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetListByTypeIdAsync(int typeId);
    }
}
