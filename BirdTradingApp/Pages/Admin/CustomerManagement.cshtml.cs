using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Admin
{
    public class CustomerManagementModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        public List<User> Users { get; set; }
        public CustomerManagementModel(IUnitOfWork unitOfWork,IHttpContextAccessor contextAccessor)
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
            Users = _unitOfWork.UserRepository.GetAllUsersExceptAdmin();
            return Page();
        }
        public IActionResult OnPostDelete(int id)
        {
            User user = _unitOfWork.UserRepository.GetUserById(id);
            _unitOfWork.UserRepository.BlockUser(id);
            TempData["NotificationMessage"] = $"Delete user {user.Name} Successfully";
            return RedirectToPage("/Admin/CustomerManagement");
        }
    }
}
