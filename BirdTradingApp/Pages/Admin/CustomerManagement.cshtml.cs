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
        public CustomerManagementModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet()
        {
            User userLogin = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
            if (userLogin == null)
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
