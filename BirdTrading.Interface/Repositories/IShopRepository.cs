using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface IShopRepository : IGenericRepository<Shop>
    {
        List<Shop> GetAll();
        Shop GetShopById(int id);
        void DeleteShop(Shop shop);
        string AddShop(Shop shop);
        string UpdateShop(Shop shop);
    }
}
