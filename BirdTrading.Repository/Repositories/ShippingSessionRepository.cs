using BirdTrading.DataAccess;
using BirdTrading.Domain.Enums;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BirdTrading.Repository.Repositories
{
    public class ShippingSessionRepository : GenericRepository<ShippingSession>, IShippingSessionRepository
    {
        public ShippingSessionRepository(AppDbContext context) : base(context)
        {
        }
        public async void CreateSessionAysnc(ShippingSession session)
        {
            _context.Set<ShippingSession>().Add(session);
            _context.SaveChanges();
        }
        public async Task<List<ShippingSession?>> GetByOrderDetailIdAndStatusAsync(int id,OrderStatus status)
        {
            return await _context.Set<ShippingSession>()
                .Where(x => x.OrderId == id && x.Status==status)
                .ToListAsync();
        }
        public  bool CheckStatus(int id,OrderStatus status)
        {
            var check = _context.Set<ShippingSession>()
                .FirstOrDefault(x => x.OrderId == id && x.Status == status);
            if (check != null)
            {
                return true;
            }
            return false;
        }
        public async Task<List<ShippingSession>> CheckSession(int id,OrderStatus status)
        {
            var check = await _context.Set<ShippingSession>()
                .Where(x => x.OrderId == id && x.Status == status).ToListAsync();
            if (check != null)
            {
                return check;
            }
            return null;
        }
        public async Task<List<ShippingSession>> CheckHistory(int id)
        {
            var check = await _context.Set<ShippingSession>()
                .Where(x => x.OrderId == id).ToListAsync();
            if (check != null)
            {
                return check;
            }
            return null;
        }
    }

}
