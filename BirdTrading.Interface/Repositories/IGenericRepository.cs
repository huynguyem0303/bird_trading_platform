using BirdTrading.Utils.Pagination;
using System.Linq.Expressions;

namespace BirdTrading.Interface.Repositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetListAsync();
        Task<Pagination<TModel>> GetPaginationsAsync(int pageIndex, int pageSize);
        Task<Pagination<TModel>> GetDescendingPaginationAsync(Expression<Func<TModel, int>> keySelector, int pageIndex, int pageSize);
        Task<TModel?> GetByIdAsync(int id);
        Task AddAsync(TModel model);
        Task AddRangeAsync(IEnumerable<TModel> models);
        void Update(TModel model);
        void Delete(TModel model);
        void DeleteRange(List<TModel> model);
    }
}
