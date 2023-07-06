using BirdTrading.Domain.Enums;
using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Repository;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using System.Web.WebPages;

namespace BirdTradingApp.Pages.Shops
{
    public class NewShopModel : PageModel
    {
        private readonly string UploadsFolderPath = "wwwroot/img/shops";
        private readonly IUnitOfWork _unitOfWork;

        public NewShopModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public Shop Shop { get; set; }
        public User User { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            var user = _unitOfWork.UserRepository.GetByIdAsync((int)userId);
            Email = user.Result.Email;
            if (user.Result.Email == null)
            {
                Email = "";
            }
            return Page();

        }
        public async Task<ActionResult> OnPostAsync(IFormFile image)
        {
            Boolean validate = true;
          
            if (Shop.Name.IsEmpty() || Shop.Name == null)
            {
                ModelState.AddModelError("Shop.Name", "Please input valid name");
                validate = false;
            }
            if (Shop.Name.Length < 3 || Shop.Name.Length > 30)
            {
                ModelState.AddModelError("Shop.Name", "Name must be between 3-30 character");
                validate = false;
            }
            if (Shop.Address.IsEmpty() || Shop.Address == null)
            {
                ModelState.AddModelError("Shop.Address", "Please input valid Address");
                validate = false;
            }
            if (Shop.Description.IsEmpty() || Shop.Description == null)
            {
                ModelState.AddModelError("Shop.Description", "Please input valid Description");
                validate = false;
            }
            if (!Regex.IsMatch(Shop.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") || Shop.Email == null)
            {
                ModelState.AddModelError("Shop.Email", "Please input valid Email.");
                validate = false;
            }

            if (!Regex.IsMatch(Shop.Phone, @"^\d+$") )
            {
                ModelState.AddModelError("Shop.Phone", "Phone must only numbers.");
                validate = false;
            }
            if (Shop.Phone.Length > 10 || Shop.Phone == null)
            {
                ModelState.AddModelError("Shop.Phone", "Please input valid phone");
                validate = false;
            }
            if (validate)
            {
                string imageUrl = null;
                User User = new User();
                var userId = HttpContext.Session.GetInt32("Id");
                var shop = _unitOfWork.ShopRepository.GetByIdAsync((int)userId);
                var currentUserLogin = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                //if (!ModelState.IsValid)
                //{
                //    return Page();
                //}
                if (image != null && image.Length > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                    var imagePath = Path.Combine(UploadsFolderPath, imageName);

                    using (var stream = System.IO.File.Create(imagePath))
                    {
                        await image.CopyToAsync(stream);
                    }
                    imageUrl = "img/Shops" + "/" + imageName;
                }


                Shop.AvatarUrl = imageUrl;
                Shop.IsBlocked = false;
                Shop.UserId = (int)userId;
                _unitOfWork.ShopRepository.UpdateUserRoleAysnc(currentUserLogin);
                _unitOfWork.ShopRepository.CreateShopAysnc(Shop);
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
