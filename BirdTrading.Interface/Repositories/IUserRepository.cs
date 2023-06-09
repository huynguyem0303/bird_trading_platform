using BirdTrading.Domain.Models;

namespace BirdTrading.Interface.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailOrPhoneAndPasswordAsync(string username, string password);
    }
}
