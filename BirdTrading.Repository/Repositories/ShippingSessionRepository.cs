using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;

namespace BirdTrading.Repository.Repositories
{
    public class ShippingSessionRepository : GenericRepository<ShippingSession>, IShippingSessionRepository
    {
        public ShippingSessionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
