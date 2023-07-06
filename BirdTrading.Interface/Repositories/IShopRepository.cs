using BirdTrading.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace BirdTrading.Interface.Repositories
{
    public interface IShopRepository : IGenericRepository<Shop>
    {
        void CreateShopAysnc(Shop shop);
        Task<Shop?> GetShopsUserIdAysnc(int id);
        Task<Shop> UpdateImageAsync(string url, int userId);
        Task<Shop> UpdateShopAsync(Shop shop);
        Task<User> UpdateUserRoleAysnc(User user);
        Task<bool> UploadFile(IFormFile file);
    }
}
