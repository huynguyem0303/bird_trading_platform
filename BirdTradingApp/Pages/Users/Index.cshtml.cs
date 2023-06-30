using BirdTrading.Repository.Repositories;
using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Utils.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace BirdTradingApp.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string UploadsFolderPath = "wwwroot/img/users";
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public User UserLogin { get; set; }
        [BindProperty]
        public IList<ShippingInformation> ShippingInformations { get; set; }
        public  IActionResult OnGet()
        {
            UserLogin = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
            if(UserLogin == null)
            {
                 return RedirectToPage("/Login/Index");
            }
            SetAll(UserLogin.Id, "personal-info-container", "");
            return Page();
        }

        public async Task<IActionResult> OnPostSaveInfor()
        {
            Boolean validate = true;
            if(UserLogin.Name.Length < 12 || UserLogin.Name.Length > 30) {
                ModelState.AddModelError("UserLogin.Name", "Name must be between 12-30 character");
                validate = false;
            }
            if(UserLogin.Name.Contains("  "))
            {
                ModelState.AddModelError("UserLogin.Name", "Please input valid name");
                validate = false;
            }
            if(!Regex.IsMatch(UserLogin.Phone, @"^\d+$")){
                ModelState.AddModelError("UserLogin.Phone", "Phone must only numbers.");
                validate = false;
            }
            if(UserLogin.Phone.Length > 10)
            {
                ModelState.AddModelError("UserLogin.Phone", "Please input valid phone");
                validate = false;
            }
            if (validate)
            {
                var newUser = new User
                {
                    Id = UserLogin.Id,
                    Name = UserLogin.Name,
                    Email = UserLogin.Email,
                    Phone = UserLogin.Phone,
                };
                UserLogin = await _unitOfWork.UserRepository.UpdateUserAsync(newUser);
                if (UserLogin != null)
                {
                    SetAll(UserLogin.Id, "personal-info-container", "Update successful!");
                    return Page();
                }
            }
            SetAll(UserLogin.Id, "personal-info-container", "");
            return Page();
        }
        
        public async Task<IActionResult> OnPostSaveImg(IFormFile image)
        {
            var imageUrl = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user").AvatarURL;
            var userId = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user").Id;

            if (image != null && image.Length > 0)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                var imagePath = Path.Combine(UploadsFolderPath, imageName);

                using (var stream = System.IO.File.Create(imagePath))
                {
                    await image.CopyToAsync(stream);
                }
                imageUrl = "img/users" + "/" + imageName;
                
            }

            UserLogin = await _unitOfWork.UserRepository.UpdateImageAsync(imageUrl, userId);
            SetAll(userId, "avatar-container", "Change Image successful!");      
            return Page();
        }

        public async Task<IActionResult> OnPostSavePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            Boolean check = true;
            string message = "";
            var currentUserLogin = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");

            if (!currentUserLogin.Password.Equals(currentPassword))
            {
                ViewData["ErrorCurrentPass"] = "Inccorrect current password";
                check = false;
            }
            if (newPassword.Contains(" "))
            {
                ViewData["ErrorNewPassword"] = "Passowrd cannot contain space";
                check = false;
            }
            if(newPassword.Length > 20 || newPassword.Length < 6)
            {
                ViewData["ErrorNewPassword"] = "Passowrd must be between 6-20 characters";
                check = false;
            }
            if (!newPassword.Equals(confirmPassword))
            {
                ViewData["ErrorConfirm"] = "Inccorrect confirm password";
                check = false;
            }
            if (check)
            {
                UserLogin = await _unitOfWork.UserRepository.UpdatePasswordAsync(newPassword, currentUserLogin.Id);
                message = "Update successful!";
            }
            else
            {
                UserLogin = currentUserLogin;
            }
            SetAll(currentUserLogin.Id, "change-password-container", message);
            return Page();
        }

        public async Task<IActionResult> OnPostModifyAddress(string name, string phone, string addressDetail, string shippingId)
        {
            var userId = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user").Id;
            ShippingInformation shippingInformation = new ShippingInformation
            {
                Name = name,
                Phone = phone,
                Address = addressDetail,
                City = Request.Form["citySelected"],
                Country = Request.Form["countrySelected"],
                IsDefaultAddress = Request.Form["default"].ToString() == "on" ? true : false
            };

            UserLogin = await _unitOfWork.UserRepository.ModifyShippingInformationAsync(shippingInformation, userId, shippingId != null ? int.Parse(shippingId) : -1);
            SetAll(UserLogin.Id, "address-container", "Update successful!");
            return Page();
        }
         public  async Task<IActionResult> OnPostSetAddressDefault(string addressId)
        {          
            UserLogin = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
            ShippingInformations = await _unitOfWork.UserRepository.SetDefaultShippingInformationUserAsync(int.Parse(addressId), UserLogin.Id);
           
            ViewData["DefaultContainer"] = "address-container";
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAddress(string addressId)
        {
            UserLogin = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
            if(await _unitOfWork.ShippingInformationRepository.DeleteShippingInformationAsync(UserLogin.Id, int.Parse(addressId)))
            {
                SetAll(UserLogin.Id, "address-container", "Address Deleted");
                return Page();
            }
            ViewData["NotificationMessage"] = "Error Occurr";
            return Page();
        }

        public void SetAll(int userId, string defaultContainer, string message)
        {
            ShippingInformations = _unitOfWork.ShippingInformationRepository.GetAllShippingInformation(userId);
            ViewData["DefaultContainer"] = defaultContainer;
            ViewData["NotificationMessage"] = message;
        }
    }
}
