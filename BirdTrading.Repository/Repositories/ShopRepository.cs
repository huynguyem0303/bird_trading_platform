using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTrading.Repository.Repositories
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        public ShopRepository(AppDbContext context) : base(context)
        {
        }

        public string AddShop(Shop shop)
        {
            if (shop == null)
            {
                return "shop is null";
            }
            bool check = _context.Shops.ToList().Any(x => x.Email == shop.Email);
            if (check)
            {
                return "Email is exist";
            }
            _context.Shops.Add(shop);
            _context.SaveChanges();
            return "Add successfully";
        }

        public void DeleteShop(Shop shop)
        {
            if (shop != null) {
                shop.IsBlocked = true;
                _context.Shops.Update(shop);
                _context.SaveChanges();
            }
        }

        public List<Shop> GetAll()
        {
            return _context.Shops.Include(x=>x.User).ToList();
        }

        public Shop GetShopById(int id)
        {
           return _context.Shops.SingleOrDefault(x=>x.Id == id);
        }

        public string UpdateShop(Shop shop)
        {
            if(shop != null)
            {
                _context.Shops.Update(shop);
                _context.SaveChanges();
                return "successfully";
            }
            return "unsuccessfully";
        }
    }
}
