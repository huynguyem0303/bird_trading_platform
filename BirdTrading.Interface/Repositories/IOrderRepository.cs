using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetListByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetListByUserIdAndStatusAsync(int userId, int status);
        Task<IEnumerable<Order>> GetListSearchAsync(int userId, string search);
    }
}
