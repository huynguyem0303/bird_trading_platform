using BirdTrading.Domain.Enums;
using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Repository.DTO;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Admin
{
    public class EditCustomerModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        [BindProperty]
        public User User { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<ShippingInformation> ShippingInfors { get; set; }
        public List<bool> AccountStatus { get; set; } = new List<bool>();
        public List<bool> AccountTemp { get; set; } = new List<bool>();
        public EditCustomerModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OnGet(int id){
            User userLogin = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
            if (userLogin == null)
            {
                return RedirectToPage("/Login/Index");
            }
            User  = _unitOfWork.UserRepository.GetUserById(id);
            UserRoles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToList().FindAll(u => u != UserRole.Admin);
            ShippingInfors = _unitOfWork.ShippingInformationRepository.GetAllShippingInformation();
            AccountStatus.Add(true);
            AccountStatus.Add(false);
            AccountTemp.Add(true);
            AccountTemp.Add(false);
            return Page();
        }
        public IActionResult OnPost() { 
            string update = _unitOfWork.UserRepository.updateUser(User);
            TempData["NotificationMessage"] = $"Update user id {User.Name} Successfully";
            return RedirectToPage("/Admin/CustomerManagement");
        }
    }
}
