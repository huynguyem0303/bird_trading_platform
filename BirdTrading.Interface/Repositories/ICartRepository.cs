using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<IEnumerable<Cart>> GetUserCartAsync(int userId);
        Task<Cart?> GetCartByUserIdAndShopIdAsync(int userId, int shopId);
    }
}
