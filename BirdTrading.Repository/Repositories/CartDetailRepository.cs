using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class CartDetailRepository : GenericRepository<CartDetail>, ICartDetailRepository
    {
        public CartDetailRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<CartDetail?> GetByIdAsync(int id)
        {
            return await _context.Set<CartDetail>()
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<CartDetail>> GetUserCartsAsync(int id)
        {
            return await _context.Set<CartDetail>()
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .Where(x => x.Cart.UserId == id)
                .ToListAsync();
        }

        public async Task<CartDetail?> GetDetailByCartIdAndProductIdAsync(int cartId, int productId)
        {
            return await _context.Set<CartDetail>().FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId);
        }

        public async Task<CartDetail?> GetDetailByUserIdAndProductIdAsync(int userId, int productId)
        {
            return await _context.Set<CartDetail>()
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.Cart.UserId == userId);
        }
    }
}
