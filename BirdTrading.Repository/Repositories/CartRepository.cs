using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Cart?> GetByIdAsync(int id)
        {
            return await _context.Set<Cart>()
                .Include(x => x.CartDetails)
                .Include(x => x.Shop)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cart?> GetCartByUserIdAndShopIdAsync(int userId, int shopId)
        {
            return await _context.Set<Cart>()
                .Include(x => x.CartDetails)
                .Include(x => x.Shop)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ShopId == shopId);
        }

        public async Task<IEnumerable<Cart>> GetUserCartAsync(int userId)
        {
            return await _context.Set<Cart>()
                .Include(x => x.CartDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.Shop)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
