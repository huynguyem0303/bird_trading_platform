using BirdTrading.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirdTrading.Interface.Repositories
{
     public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailOrPhoneAndPasswordAsync(string username, string password);
        Task<User> CreateUserAsync(string username, string password, String fullname);
        Task<User> ModifyShippingInformationAsync(ShippingInformation shippingInformation, int userId, int shippingInformationId);
        Task<IList<ShippingInformation>> SetDefaultShippingInformationUserAsync(int shippingInformationId, int userId);
        Task<User> UpdateImageAsync(string url, int userId);
        Task<User> UpdatePasswordAsync(string password, int userId);
        Task<User> UpdateUserAsync(User user);

    }
}
