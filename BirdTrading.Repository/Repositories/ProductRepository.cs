using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using BirdTrading.Utils.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Pagination<Product>> SearchProductPagingAsync(string search, int pageIndex, int pageSize)
        {
            var products = _context.Set<Product>()
                .Where(x => x.Name.ToUpper().Contains(search.ToUpper()) && x.IsRemoved == false)
                .AsQueryable();
            //
            var totalCount = await products.CountAsync();
            var items = await products.AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToListAsync();

            var result = new Pagination<Product>()
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = totalCount
            };
            return result;
        }
    }
}
