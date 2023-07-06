using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Set<Category>()
                .Include(x => x.Type)
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Category?> GetCategoryNameAysnc(int id)
        {
            return await _context.Set<Category>()
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Category>> GetListByTypeIdAsync(int typeId)
        {
            return await _context.Set<Category>()
                .Include(x => x.Products)
                .Where(x => x.TypeId == typeId)
                .ToListAsync();
        }
    }
}
