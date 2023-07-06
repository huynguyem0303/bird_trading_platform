using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using System.Web.WebPages;

namespace BirdTradingApp.Pages.Shops
{
    public class EditShopModel : PageModel
    {
        private readonly string UploadsFolderPath = "wwwroot/img/shops";
        private readonly IUnitOfWork _unitOfWork;

        public EditShopModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public Shop Shop { get; set; }
        public int? Session { get; set; }

        public IActionResult OnGet()
        {
            Session = HttpContext.Session.GetInt32("Id");
            var userId = HttpContext.Session.GetInt32("Id");
            Shop = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)userId).Result;
            var shop = Shop.AvatarUrl;
            return Page();

        }
        public async Task<IActionResult> OnPostSaveInfo()
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

            if (!Regex.IsMatch(Shop.Phone, @"^\d+$"))
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
                var shop = new Shop
                {
                    Id = Shop.Id,
                    Name = Shop.Name,
                    Email = Shop.Email,
                    Phone = Shop.Phone,
                    Address = Shop.Address,
                    Description = Shop.Description,
                };
                Shop = await _unitOfWork.ShopRepository.UpdateShopAsync(shop);
                return RedirectToPage("/Shops/Index");
            }
            var userId = HttpContext.Session.GetInt32("Id");
            Shop = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)userId).Result;
            return Page();
        }
        public async Task<ActionResult> OnPostSaveImg(IFormFile image)
        {
            Session = HttpContext.Session.GetInt32("Id");
            var users = _unitOfWork.UserRepository.GetUserById((int)Session);
       
            var imageUrl = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)Session).Result.AvatarUrl;
            if (image != null )
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                var imagePath = Path.Combine(UploadsFolderPath, imageName);

                using (var stream = System.IO.File.Create(imagePath))
                {
                    await image.CopyToAsync(stream);
                }
                imageUrl = "img/shops" + "/" + imageName;

            }

            Shop = await _unitOfWork.ShopRepository.UpdateImageAsync(imageUrl, users.Id);
            return Page();
        }
    }
}
