using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class CategoryTypeRepository : GenericRepository<CategoryType>, ICategoryTypeRepository
    {
        public CategoryTypeRepository(AppDbContext context) : base(context)
        { }

        public override async Task<IEnumerable<CategoryType>> GetListAsync()
        {
            return await _context.Set<CategoryType>()
                .Include(x => x.Categories)
                .ToListAsync();
        }
    }
}
