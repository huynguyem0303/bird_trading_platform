using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.WebPages;

namespace BirdTradingApp.Pages.Products
{
    public class AddProductModel : PageModel
    {
        private readonly string UploadsFolderPath = "wwwroot/img";
        private readonly IUnitOfWork _unitOfWork;

        public AddProductModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public Product Product { get; set; }
        public string SelectedCategoryType { get; set; }
        public List<SelectListItem> CategoryType { get; set; }
        public string SelectedCategory { get; set; }
        public List<SelectListItem> Category { get; set; }
        public string msg { get; set; }
        public int? Session { get; set; }
        public IActionResult OnGet(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            ViewData["Category"] = new SelectList(_unitOfWork.CategoryRepository.GetListAsync().Result, "Id", "Name");
            return Page();
        }


        public async Task<ActionResult> OnPostAsync(IFormFile image)
        {
            Session = HttpContext.Session.GetInt32("Id");
            ViewData["Category"] = new SelectList(_unitOfWork.CategoryRepository.GetListAsync().Result, "Id", "Name");
            Boolean validate = true;
            if (image == null)
            {
                msg = "img Url cannot be null or empty";
                return Page();
            }
            if (Product.Name.IsEmpty() || Product.Name == null)
            {
                ModelState.AddModelError("Product.Name", "Please Name cannot be null or empty");
                validate = false;
            }
            
            if (Product.Name != null)
            {
                if (Product.Name.Length < 3 || Product.Name.Length > 30)
                {
                    ModelState.AddModelError("Product.Name", "Name must be between 3-30 character");
                    validate = false;

                }
                if (Product.OriginalPrice <= 0 || Product.OriginalPrice >= 1000000)
                {
                    ModelState.AddModelError("Product.OriginalPrice", "Please input valid Price (Price number is from 0-1000000)");
                    validate = false;

                }
                if (Product.Quantity <= 0 || Product.Quantity >= 1000000)
                {
                    ModelState.AddModelError("Product.Quantity", "Please input valid Quantity (Quantity number is from 0-1000000)");
                    validate = false;
                    return Page();
                }

                if (Product.Description.IsEmpty() || Product.Description == null)
                {
                    ModelState.AddModelError("Product.Description", "Please input valid Description");
                    validate = false;
                    return Page();
                }
            }
            if (validate)
            {
                string imageUrl = null;
                User User = new User();
                var userId = HttpContext.Session.GetInt32("Id");
                var shopid = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)userId).Result.Id;
                var currentUserLogin = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                //if (!ModelState.IsValid)
                //{
                //    return Page();
                //}
                if (image != null && image.Length > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var cate = _unitOfWork.CategoryRepository.GetCategoryNameAysnc(Product.CategoryId).Result.Name.Trim().ToLower();
                    var folder = Path.Combine(UploadsFolderPath, cate);
                    var imagePath = Path.Combine(folder, imageName);
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    var imagePath1 = Path.Combine(folder, imageName);
                    using (var stream = System.IO.File.Create(imagePath))
                    {
                        await image.CopyToAsync(stream);
                    }
                    imageUrl = "img/" + cate + "/" + imageName;
                }

                Product.ImageUrl = imageUrl;
                Product.IsRemoved = false;
                Product.ShopId = shopid;
                _unitOfWork.ProductRepository.CreateProductAysnc(Product);
                return RedirectToPage("/Shops/AllProduct");
            }
            ViewData["Category"] = new SelectList(_unitOfWork.CategoryRepository.GetListAsync().Result, "Id", "Name");
            return Page();

        }
        public async Task<IActionResult> OnPostProductList()
        {
            return RedirectToPage("/Shops/AllProduct");
        }
    }
}
