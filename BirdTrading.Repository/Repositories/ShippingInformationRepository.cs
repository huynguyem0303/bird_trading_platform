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

        public async Task<bool> DeleteShippingInformationAsync(int userId, int shippingInformationId)
        {
            var shippingInformations =  GetAllShippingInformation(userId);
            if(shippingInformations != null)
            {
                foreach (var address in shippingInformations)
                {
                    if (address.Id == shippingInformationId)
                    {
                        _context.Set<ShippingInformation>().Remove(address);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }                    
            return false;
        }

        public  IList<ShippingInformation> GetAllShippingInformation(int userId)
        {
            return  _context.ShippingInformation
                .Include(x => x.Users)
                .Where(x => x.Users.Any(u => u.Id == userId)).ToList();
        }

        public List<ShippingInformation> GetAllShippingInformation()
        {
            return _context.ShippingInformation.ToList();
        }

        public async Task<ShippingInformation?> GetDefaultShippingInformationAsync(int userId)
        {
            return await _context.Set<ShippingInformation>()
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.IsDefaultAddress && x.Users.Any(u => u.Id == userId));
        }
        public async Task<ShippingInformation?> GetShippingInformationAsync(int Id)
        {
            return await _context.Set<ShippingInformation>()
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == Id );
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
