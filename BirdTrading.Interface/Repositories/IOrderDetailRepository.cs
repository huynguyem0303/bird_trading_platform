using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        Task<IEnumerable<int>> GetTop8PopularProducts();
        Task<IEnumerable<OrderDetail>> GetUserCartsAsync(int id);
    }
}
