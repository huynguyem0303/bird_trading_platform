using BirdTrading.Repository.Repositories;
using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using BirdTradingApp.Services;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public  async Task<IActionResult> OnGet()
        {
            string role = HttpContext.Session.GetString("Role");
            int id = (int)HttpContext.Session.GetInt32("Id");
         
            if(role == null)
            {
                 return RedirectToPage("/Login/Index");
            }
            UserLogin = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            SetAll(id, "personal-info-container", "");
            TempData["error"] = TempData["msg"];
            return Page();
        }

        public async Task<IActionResult> OnPostSaveInfor()
        {
            Boolean validate = true;
            int userId = UserLogin.Id;
            if(UserLogin.Name.Length < 6 || UserLogin.Name.Length > 30) {
                ModelState.AddModelError("UserLogin.Name", "Name must be between 6-30 character");
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
            if( !Regex.IsMatch(UserLogin.Email, @"@"))
            {
                ModelState.AddModelError("UserLogin.Email", "Please input valid email");
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
                    SetAll(userId, "personal-info-container", "Update successful");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError("UserLogin.Email", "The new email is already in use");
                }
            }
            UserLogin = _unitOfWork.UserRepository.GetUserById(userId);
            SetAll(userId, "personal-info-container", "");
            return Page();
        }
        
        public async Task<IActionResult> OnPostSaveImg(IFormFile image)
        {
            var userId = (int)HttpContext.Session.GetInt32("Id");
            UserLogin = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var imageUrl = UserLogin.AvatarURL;

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
            SetAll(userId, "avatar-container", "Change Image successful");      
            return Page();
        }

        public async Task<IActionResult> OnPostSavePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            Boolean check = true;
            string message = "";

            int id = (int)HttpContext.Session.GetInt32("Id");
            var currentUserLogin = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

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
            if(newPassword.Length > 30 || newPassword.Length < 6)
            {
                ViewData["ErrorNewPassword"] = "Passowrd must be between 6-30 characters";
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
            SetAll( id, "change-password-container", message);
            return Page();
        }

        public async Task<IActionResult> OnPostModifyAddress(string name, string phone, string addressDetail, string shippingId)
        {
            var userId = (int)HttpContext.Session.GetInt32("Id");
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
            SetAll(userId, "address-container", shippingId == null ? "Add successful" : "Update successful!");
            return Page();
        }
         public  async Task<IActionResult> OnPostSetAddressDefault(string addressId)
        {
            var userId = (int)HttpContext.Session.GetInt32("Id");
            UserLogin = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            ShippingInformations = await _unitOfWork.UserRepository.SetDefaultShippingInformationUserAsync(int.Parse(addressId), userId);
           
            ViewData["DefaultContainer"] = "address-container";
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAddress(string addressId)
        {
            var userId = (int)HttpContext.Session.GetInt32("Id");
            UserLogin = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (await _unitOfWork.ShippingInformationRepository.DeleteShippingInformationAsync(userId, int.Parse(addressId)))
            {
                SetAll(userId, "address-container", "Address delete successful");
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
