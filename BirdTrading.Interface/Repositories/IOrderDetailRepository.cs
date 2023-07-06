using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        Task<List<OrderDetail?>> GetByOrderIdAsync(int id);
        Task<List<OrderDetail?>> GetByProductIdAsync(int id);
        Task<IEnumerable<int>> GetTop8PopularProducts();
        Task<IEnumerable<OrderDetail>> GetUserCartsAsync(int id);
    }
}
