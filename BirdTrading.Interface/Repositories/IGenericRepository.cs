using BirdTrading.Utils.Pagination;

namespace BirdTrading.Interface.Repositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetListAsync();
        Task<Pagination<TModel>> GetPaginationsAsync(int pageIndex, int pageSize);
        Task<TModel?> GetByIdAsync(int id);
        Task AddAsync(TModel model);
        void Update(TModel model);
        void Delete(TModel model);
    }
}
