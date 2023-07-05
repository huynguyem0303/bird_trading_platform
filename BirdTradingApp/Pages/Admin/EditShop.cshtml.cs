using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace BirdTradingApp.Pages.Admin
{
    public class EditShopModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        [BindProperty]
        public Shop AddShopDTO { get; set; }
        public List<User> Users { get; set; }
        public List<bool> AccountStatus { get; set; } = new List<bool>();
        public EditShopModel(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }
        public IActionResult OnGet(int id)
        {
            string role = _contextAccessor.HttpContext.Session.GetString("Role");
            if (role is null || !role.Equals("Admin"))
            {
                return RedirectToPage("/Login/Index");
            }
            AddShopDTO = _unitOfWork.ShopRepository.GetShopById(id);
            List<Shop> shops = _unitOfWork.ShopRepository.GetAll();
            List<int> shopIds = (from s in shops
                                 select s.UserId).ToList();
            Users = _unitOfWork.UserRepository.GetAllUsersExceptAdmin().FindAll(x => x.Role == BirdTrading.Domain.Enums.UserRole.ShopOwner && shopIds.Contains(x.Id) == false);
            User user = _unitOfWork.UserRepository.GetUserById(AddShopDTO.UserId);
            Users.Add(user);
            AccountStatus.Add(true);
            AccountStatus.Add(false);
            return Page();
        }
        public IActionResult OnPost()
        {
            _unitOfWork.ShopRepository.UpdateShop(AddShopDTO);
            TempData["NotificationMessage"] = $"Update shop {AddShopDTO.Name} Successfully";
            return RedirectToPage("/Admin/ShopManagement");
        }
    }
}
