using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface IShippingInformationRepository : IGenericRepository<ShippingInformation>
    {
        Task<ShippingInformation?> GetDefaultShippingInformationAsync(int userId);
        Task<IEnumerable<ShippingInformation>> GetUserShippingInformationAsync(int userId);

        IList<ShippingInformation> GetAllShippingInformation(int userId);
        Task<bool> DeleteShippingInformationAsync(int userId, int shippingInformationId);
        Task<ShippingInformation?> GetShippingInformationAsync(int Id);
    }
}
