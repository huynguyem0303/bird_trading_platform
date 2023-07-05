using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetListByUserIdAndStatusAsync(int userId, int status)
        {
            return await _context.Set<Order>()
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Shop)
                .Include(x => x.ShippingSessions)
                .Where(x => x.UserId == userId && x.ShippingSessions.Any(s => (int)s.Status == status))
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetListByUserIdAsync(int userId)
        {
            return await _context.Set<Order>()
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Shop)
                .Include(x => x.ShippingSessions)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetListSearchAsync(int userId, string search)
        {
            return await _context.Set<Order>()
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Shop)
                .Include(x => x.ShippingSessions)
                .Where(x => x.UserId == userId &&
                (x.OrderDetails.Any(o => o.Product.Name.ToLower().Contains(search.ToLower())) ||
                x.OrderDetails.Any(o => o.Product.Shop.Name.ToLower().Contains(search.ToLower())) ||
                x.Id.ToString().Contains(search)))
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public override async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Set<Order>()
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.ShippingInformation)
                .Include(x => x.ShippingSessions)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public decimal[] DoStatics()
        {
            decimal[] result = new decimal[7];
            for(int i = 0; i < 7; i++)
            {
                result[i] = GetAll().FindAll(x => x.OrderDate.Month == (i + 1)).Sum(x => x.Total);
            }
            return result;
        }
    }
}
