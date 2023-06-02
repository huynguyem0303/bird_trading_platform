using BirdTrading.DataAccess;
using BirdTrading.Interface.Repositories;
using BirdTrading.Utils.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TModel model)
        {
            await _context.Set<TModel>().AddAsync(model);
        }

        public void Delete(TModel model)
        {
            _context.Remove(model);
        }

        public async Task<TModel?> GetByIdAsync(int id)
        {
            return await _context.Set<TModel>().FindAsync(id);
        }

        public async Task<IEnumerable<TModel>> GetListAsync()
        {
            return await _context.Set<TModel>().ToListAsync();
        }

        public async Task<Pagination<TModel>> GetPaginationsAsync(int pageIndex, int pageSize)
        {
            var totalCount = await _context.Set<TModel>().CountAsync();
            var items = await _context.Set<TModel>().AsNoTracking()
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

            var result = new Pagination<TModel>()
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalCount
            };
            return result;
        }

        public void Update(TModel model)
        {
            _context.Set<TModel>().Update(model);
        }
    }
}
