using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;

namespace BirdTrading.Repository.Repositories
{
    public class ShippingInformationRepository : GenericRepository<ShippingInformation>, IShippingInformationRepository
    {
        public ShippingInformationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
