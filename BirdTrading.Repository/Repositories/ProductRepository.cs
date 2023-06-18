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

        public override async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Set<Product>()
                .Include(x => x.Shop)
                .Include(x => x.Category)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Pagination<Product>> GetPaginationsAsync(int pageIndex, int pageSize)
        {
            var totalCount = await _context.Set<Product>().CountAsync();
            var items = await _context.Set<Product>()
                .Include(x => x.OrderDetails)
                .AsNoTracking()
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

        public async Task<Pagination<Product>> GetProductPagingByCategoryAsync(int category, int pageIndex, int pageSize)
        {
            var products = _context.Set<Product>()
                .Where(x => x.CategoryId == category && x.IsRemoved == false)
                .Include(x => x.Category)
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

        public async Task<Pagination<Product>> GetProductPagingByCategoryTypeAsync(int categoryType, int pageIndex, int pageSize)
        {
            var products = _context.Set<Product>()
                .Where(x => x.Category.TypeId == categoryType && x.IsRemoved == false)
                .Include(x => x.Category)
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

        public async Task<IEnumerable<Product>> GetTop8RelateProductAsync(int categoryType, int productId)
        {
            var relateProducts = await _context.Set<Product>()
                .Include(x => x.Shop)
                .Include(x => x.Category)
                .Include(x => x.OrderDetails)
                .Where(x => x.Category.TypeId == categoryType && x.Id != productId)
                .ToListAsync();
            var random = new Random();
            relateProducts = relateProducts.OrderBy(x => random.NextInt64()).ToList();
            return relateProducts.Take(8);
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
