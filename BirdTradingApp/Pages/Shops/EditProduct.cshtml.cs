using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Repository.Repositories;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using System.Web.WebPages;

namespace BirdTradingApp.Pages.Shops
{
    public class EditProductModel : PageModel
    {
        private readonly string UploadsFolderPath = "wwwroot/img";
        private readonly IUnitOfWork _unitOfWork;

        public EditProductModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]

        public Product Product { get; set; }
        public string SelectedCategoryType { get; set; }
        public List<SelectListItem> CategoryType { get; set; }
        public string SelectedCategory { get; set; }
        public List<SelectListItem> Category { get; set; }
        public static int cateid { get; set; }
        public static int productid { get; set; }
        public static int Pid { get; set; }
        public int? Session { get; set; }
        public string msg { get; set; }
        public IActionResult OnGet(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            Pid = id;
            if (id == null)
            {
                return NotFound();
            }
          
            Product =_unitOfWork.ProductRepository.GetByIdAsync(id).Result;
            cateid = Product.CategoryId;
            productid = Product.Id;
            if (Product == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_unitOfWork.CategoryRepository.GetListAsync().Result, "Id", "Name");
            return Page();
        }
        public async Task<IActionResult> OnPostSaveInfo()
        {
            Session = HttpContext.Session.GetInt32("Id");
            Boolean validate = true;
            if (Product.Name.IsEmpty() || Product.Name == null)
            {
                ModelState.AddModelError("Product.Name", "Please input valid Name");
                validate = false;
            }
            if (Product.Name.Length < 3 || Product.Name.Length > 30)
            {
                ModelState.AddModelError("Product.Name", "Name must be between 3-30 character");
                validate = false;
            }
            if (Product.OriginalPrice <= 0 || Product.OriginalPrice >= 1000000)
            {
                ModelState.AddModelError("Product.OriginalPrice", "Please input valid Price (OriginalPrice number is from 0-1000000)");
                validate = false;
            }
            if (Product.Quantity <= 0 || Product.Quantity >= 1000000)
            {
                ModelState.AddModelError("Product.Quantity", "Please input valid Quantity (Quantity number is from 0-1000000)");
                validate = false;
            }
            if (Product.Description.IsEmpty() || Product.Description == null)
            {
                ModelState.AddModelError("Product.Description", "Please input valid Description");
                validate = false;
            }
                if (validate)
            {
                var product = new Product
                {
                    Id = Product.Id,
                    Name = Product.Name,
                    OriginalPrice = Product.OriginalPrice,
                    Quantity = Product.Quantity,
                    CategoryId = Product.CategoryId,
                    Description = Product.Description,
                };
                Product = await _unitOfWork.ProductRepository.UpdateProductgAsync(product);
                return RedirectToPage("/Shops/AllProduct");
            }
          
            Product = _unitOfWork.ProductRepository.GetByIdAsync(Pid).Result;
            cateid = Product.CategoryId;
            productid = Product.Id;
            if (Product == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_unitOfWork.CategoryRepository.GetListAsync().Result, "Id", "Name");
            return Page();
        }
        public async Task<ActionResult> OnPostSaveImg(IFormFile imageproduct)
        {
           
             if (imageproduct == null)
            {
                Product = _unitOfWork.ProductRepository.GetByIdAsync(Pid).Result;
                if (Product.ImageUrl == null)
                {
                    msg = "img Url cannot be null or empty";
                }
            }
            Session = HttpContext.Session.GetInt32("Id");
            var users = _unitOfWork.UserRepository.GetUserById((int)Session);
            var imageUrl = _unitOfWork.ProductRepository.GetByIdAsync((int)Pid).Result.ImageUrl;
            if (imageproduct != null)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageproduct.FileName);
                var cate = _unitOfWork.CategoryRepository.GetCategoryNameAysnc(cateid).Result.Name.Trim().ToLower();
                var folder = Path.Combine(UploadsFolderPath, cate);
                var imagePath = Path.Combine(folder, imageName);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var imagePath1 = Path.Combine(folder, imageName);
                using (var stream = System.IO.File.Create(imagePath))
                {
                    await imageproduct.CopyToAsync(stream);
                }
                imageUrl = "img/" + cate + "/" + imageName;

            }
            ViewData["Category"] = new SelectList(_unitOfWork.CategoryRepository.GetListAsync().Result, "Id", "Name");
            Product = await _unitOfWork.ProductRepository.UpdateImageAsync(imageUrl, productid);
            return Page();
        }
        public async Task<IActionResult> OnPostProductList()
        {
            return RedirectToPage("/Shops/AllProduct");
        }

    }
   
}
