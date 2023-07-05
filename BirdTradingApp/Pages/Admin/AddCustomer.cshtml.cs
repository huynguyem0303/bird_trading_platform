using BirdTrading.Domain.Enums;
using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Repository.DTO;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;

namespace BirdTradingApp.Pages.Admin
{
    public class AddCustomerModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        [BindProperty]
        public AddUserDTO AddCustomerDTO { get; set; }
        public List<UserRole> UserRoles { get; set; } 
        public List<ShippingInformation> ShippingInfors { get; set; }
        public AddCustomerModel(IUnitOfWork unitOfWork,IHttpContextAccessor contextAccessor) {
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
            UserRoles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToList().FindAll(u => u != UserRole.Admin);
            ShippingInfors = _unitOfWork.ShippingInformationRepository.GetAllShippingInformation();
            return Page();
        }
        public IActionResult OnPost() { 
           AddCustomerDTO.IsBlocked = false;
           AddCustomerDTO.IsTempUser = true;
            User user = new User
            {
                Email = AddCustomerDTO.Email,
                Name = AddCustomerDTO.Name,
                Phone = AddCustomerDTO.Phone,
                Password = AddCustomerDTO.Password,
                Role = AddCustomerDTO.Role,
                AvatarURL = AddCustomerDTO.AvatarURL,
                IsBlocked = AddCustomerDTO.IsBlocked,
                IsTempUser = AddCustomerDTO.IsTempUser,
                ShippingInforId = AddCustomerDTO.ShippingInforId,
            };
            string result =  _unitOfWork.UserRepository.AddUser(user);
            if(result.Equals("Email is exist"))
            {
                UserRoles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToList().FindAll(u => u != UserRole.Admin);
                ShippingInfors = _unitOfWork.ShippingInformationRepository.GetAllShippingInformation();
                ViewData["EmailError"] = "Email is exist";
                return Page();
            }   
            TempData["NotificationMessage"] = "Add new user successfully";
            return RedirectToPage("/Admin/CustomerManagement");
        }
    }
}
