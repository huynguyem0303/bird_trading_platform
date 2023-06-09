using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface ICartDetailRepository : IGenericRepository<CartDetail>
    {
        Task<IEnumerable<CartDetail>> GetUserCartsAsync(int id);
        Task<CartDetail?> GetDetailByCartIdAndProductIdAsync(int cartId, int productId);
    }
}
