using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<int>> GetTop8PopularProducts()
        {
            var products = await _context.Set<OrderDetail>()
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductsId = x.Key,
                    Counts = x.Count(),
                })
                .Take(8)
                .OrderByDescending(x => x.Counts)
                .ToListAsync();
            var productIds = new List<int>();
            foreach (var item in products)
            {
                productIds.Add(item.ProductsId);
            }
            return productIds;
        }

        public async Task<IEnumerable<OrderDetail>> GetUserCartsAsync(int id)
        {
            return await _context.Set<OrderDetail>()
                .Where(x => x.Order.UserId == id)
                .Include(x => x.Order)
                .Include(x => x.Product)
                .ToListAsync();
        }

        public override async Task<OrderDetail?> GetByIdAsync(int id)
        {
            return await _context.Set<OrderDetail>()
                .Include(x => x.Order)
                .ThenInclude(x => x.ShippingSessions)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<OrderDetail?>> GetByProductIdAsync(int id)
        {
            return await _context.Set<OrderDetail>()
                .Where(x => x.ProductId == id)
                .ToListAsync();
        }
        public async Task<List<OrderDetail?>> GetByOrderIdAsync(int id)
        {
            return await _context.Set<OrderDetail>()
                .Include (x => x.Product)
                .Where(x => x.OrderId == id)
                .ToListAsync();
        }

    }
}
