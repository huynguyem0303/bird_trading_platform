using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface IShippingInformationRepository : IGenericRepository<ShippingInformation>
    {
        Task<ShippingInformation?> GetDefaultShippingInformationAsync(int userId);
        Task<IEnumerable<ShippingInformation>> GetUserShippingInformationAsync(int userId);
    }
}
