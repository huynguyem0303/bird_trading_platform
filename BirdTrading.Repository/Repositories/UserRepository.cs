using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailOrPhoneAndPasswordAsync(string username, string password)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(x => (x.Email == username || x.Phone == username)
                && x.Password == password);
        }
    }
}
