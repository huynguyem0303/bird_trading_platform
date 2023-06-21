using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class ShippingInformationRepository : GenericRepository<ShippingInformation>, IShippingInformationRepository
    {
        public ShippingInformationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ShippingInformation?> GetDefaultShippingInformationAsync(int userId)
        {
            return await _context.Set<ShippingInformation>()
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.IsDefaultAddress && x.Users.Any(u => u.Id == userId));
        }

        public async Task<IEnumerable<ShippingInformation>> GetUserShippingInformationAsync(int userId)
        {
            return await _context.Set<ShippingInformation>()
                .Include(x => x.Users)
                .Where(x => x.Users.Any(u => u.Id == userId))
                .ToListAsync();
        }
    }
}
