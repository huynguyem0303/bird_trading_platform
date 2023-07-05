using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Admin
{
    public class ShopManagementModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        public List<Shop> Shops { get; set; }
        public ShopManagementModel(IUnitOfWork unitOfWork,IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }
        public IActionResult OnGet()
        {
            string role = _contextAccessor.HttpContext.Session.GetString("Role");
            if (role is null || !role.Equals("Admin"))
            {
                return RedirectToPage("/Login/Index");
            }
            Shops = _unitOfWork.ShopRepository.GetAll();
            return Page();
        }
        public IActionResult OnPostDelete(int id)
        {
            Shop shop = _unitOfWork.ShopRepository.GetShopById(id);
            if (shop != null)
            {
               _unitOfWork.ShopRepository.DeleteShop(shop);
                TempData["NotificationMessage"] = "Blocked a shop successfully";
            }
            return RedirectToPage("/Admin/ShopManagement");
        }
    }
}
