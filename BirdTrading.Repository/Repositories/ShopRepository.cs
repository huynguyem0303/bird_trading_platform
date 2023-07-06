using Azure.Core;
using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;




namespace BirdTrading.Repository.Repositories
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        public ShopRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<Shop?> GetShopsUserIdAysnc(int id)
        {
            return await _context.Set<Shop>()
                .Where(x => x.UserId == id).FirstOrDefaultAsync();
        }
       
        public async void CreateShopAysnc(Shop shop)
        {
            _context.Set<Shop>().Add(shop);
            _context.SaveChanges();
        }
        public async Task<User> UpdateUserRoleAysnc(User user)
        {
            var currentUser = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == user.Id);
            if (currentUser != null)
            {
                currentUser.Role = Domain.Enums.UserRole.ShopOwner;
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return currentUser;
            }
            return null;
        }
        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "/BirdTradingApp/wwwroot/img/shops"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }
        public async Task<Shop> UpdateImageAsync(string url, int userId)
        {
            var currentUser = await _context.Set<Shop>().FirstOrDefaultAsync(u => u.UserId == userId);
            if (currentUser != null)
            {
                currentUser.AvatarUrl = url;
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return currentUser;
        }
        public async Task<Shop> UpdateShopAsync(Shop shop)
        {
            var currentShop = await _context.Set<Shop>().FirstOrDefaultAsync(u => u.Id == shop.Id);
            if (currentShop != null)
            {
                currentShop.Name = shop.Name;
                currentShop.Email = shop.Email;
                currentShop.Phone = shop.Phone;
                currentShop.Address = shop.Address;
                currentShop.Description = shop.Description;

                _context.Entry(currentShop).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return currentShop;
            }
            return null;
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
