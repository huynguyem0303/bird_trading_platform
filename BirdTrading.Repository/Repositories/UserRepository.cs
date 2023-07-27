         using BirdTrading.DataAccess;
using BirdTrading.Domain.Models;
using BirdTrading.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace BirdTrading.Repository.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public string AddUser(User user)
        {
            if(user == null)
            {
                return "user is null";
            }
            bool check = _context.Users.ToList().Any(x => x.Email == user.Email);
            if(check) {
                return "Email is exist";
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return "Add successfully";
        }

        public void BlockUser(int id)
        {
            User user = _context.Users.FirstOrDefault(x => x.Id == id);
            if(user == null)
            {
                return;
            }
            user.IsBlocked = true;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public async Task<User> CreateUserAsync(string username, string password, String fullname)
        {
            User user = await _context.Set<User>().FirstOrDefaultAsync(u => (u.Email == username || u.Phone == username));
            if (user == null)
            {
                user = new User
                {
                    Email = username.Contains("@") ? username : "",
                    Phone = username.Contains("@") ? "" : username,
                    Name = fullname,
                    Password = password,
                    Role = Domain.Enums.UserRole.Customer,
                    AvatarURL= "",
                    IsTempUser = false,
                    IsBlocked = false,
                    ShippingInforId = 0
                };
                 user.Id =  _context.Set<User>().Add(user).Entity.Id;
                _context.SaveChanges();
                return user; 
            }
            return null;
        }

        public int[] DoStatic()
        {
            int[] result = new int[2];
            int active = _context.Users.ToList().FindAll(u => u.Role != BirdTrading.Domain.Enums.UserRole.Admin).FindAll(x=>x.IsBlocked == false).Count;
            result[0] = active;
            int blocked = _context.Users.ToList().FindAll(u => u.Role != BirdTrading.Domain.Enums.UserRole.Admin).FindAll(x => x.IsBlocked == true).Count;
            result[1] = blocked;
            return result;
        }

        public List<User> GetAllUsersExceptAdmin()
        {
            return _context.Users.ToList().FindAll(u => u.Role != BirdTrading.Domain.Enums.UserRole.Admin);
        }
        public List<User> GetAllUsersExceptAdminandNoShop() {
            List<Shop> shops = _context.Shops.ToList();
            List<int> userIds = (from s in shops
                                select s.Id).ToList();
            List<User> users = _context.Users.Include(x => x.Shop).ToList().FindAll(u => u.Role == BirdTrading.Domain.Enums.UserRole.ShopOwner && userIds.Contains(u.Id) == true);
            return users;

        }
        public async Task<User?> GetUserByEmailOrPhoneAndPasswordAsync(string username, string password)
        {
            return await _context.Set<User>().
                FirstOrDefaultAsync(x => (x.Email == username || x.Phone == username)
                && x.Password == password);
        }
        public User GetUserById(int id)
        {
            return _context.Users.ToList().SingleOrDefault(x=>x.Id == id);
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> ModifyShippingInformationAsync(ShippingInformation shippingInformation, int userId, int shippingInformationId)
        {
            var currentUser = await _context.Users
         .Include(u => u.ShippingInformations)
         .FirstOrDefaultAsync(u => u.Id == userId);

            if (currentUser != null)
            {
                try
                {
                    var shippingInformationsList = currentUser.ShippingInformations.ToList();

                    if (shippingInformationId == -1)
                    {
                        if (shippingInformation.IsDefaultAddress)
                        {
                            foreach (var address in shippingInformationsList)
                            {
                                address.IsDefaultAddress = false;
                            }
                        }
                        shippingInformationsList.Add(shippingInformation);
                      
                    }
                    else
                    {
                        foreach (var address in shippingInformationsList)
                        {
                            if (shippingInformation.IsDefaultAddress)
                            {
                                address.IsDefaultAddress = false;
                            }
                            if (address.Id == shippingInformationId)
                            {
                                address.Name = shippingInformation.Name;
                                address.Address = shippingInformation.Address;
                                address.City = shippingInformation.City;
                                address.Phone = shippingInformation.Phone;
                                address.IsDefaultAddress = shippingInformation.IsDefaultAddress;
                            }
                        }
                    }
                    _context.Entry(currentUser).State = EntityState.Modified;
                    currentUser.ShippingInformations = shippingInformationsList;
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }              
            }
            return currentUser;
        }

        public async Task<IList<ShippingInformation>> SetDefaultShippingInformationUserAsync(int shippingInformationId, int userId)
        {

            var currentUser =  _context.Users.Include(u => u.ShippingInformations).FirstOrDefault(u => u.Id == userId);
            var shippingInformationsList = currentUser.ShippingInformations.ToList();
            foreach (var address in shippingInformationsList)
                {
                       address.IsDefaultAddress = false;
                if (address.Id == shippingInformationId)
                    {
                        address.IsDefaultAddress = true;
                    } 
                }
                currentUser.ShippingInformations = shippingInformationsList;
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            return shippingInformationsList;
        }

        public async Task<User> UpdateImageAsync(string url, int userId)
        {
            var currentUser = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser != null)
            {
                currentUser.AvatarURL = url;
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return currentUser;
        }

        public async Task<User> UpdatePasswordAsync(string password, int userId)
        {
            var currentUser = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser != null)
            {
                currentUser.Password = password;
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return currentUser;
        }

        public string updateUser(User user)
        {
            if (user == null)
            {
                return "user is null";
            }
            _context.Users.Update(user);
            _context.SaveChanges();
            return "Update successfully";
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var currentUser = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == user.Id);
            if (currentUser != null)
            {
                var checkValidEmail = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == user.Email);
                if (checkValidEmail == null || checkValidEmail.Id == user.Id)
                {
                    currentUser.Name = user.Name;
                    currentUser.Email = user.Email;
                    currentUser.Phone = user.Phone;
                    _context.Entry(currentUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return currentUser;
                }           
            }
            return null;
        }
        public async Task<User> UpdateUserRole(User user)
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
    }

}
