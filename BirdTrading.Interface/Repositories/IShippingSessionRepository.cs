using BirdTrading.Domain.Enums;
using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface IShippingSessionRepository : IGenericRepository<ShippingSession>
    {
        Task<List<ShippingSession>> CheckHistory(int id);
        Task<List<ShippingSession>> CheckSession(int id, OrderStatus status);
        bool CheckStatus(int id, OrderStatus status);
        void CreateSessionAysnc(ShippingSession session);
        Task<List<ShippingSession?>> GetByOrderIdAndStatusAsync(int id, OrderStatus status);
    }
}
