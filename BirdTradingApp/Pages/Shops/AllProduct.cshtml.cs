using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BirdTradingApp.Pages.Shops
{
    public class AllProductModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public AllProductModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public SelectList Options { get; set; }
        public List<Product> Product { get; set; }
        public Product status { get; set; }
        public string CurrentFilter { get; set; }

        public int? Session { get; set; }
        public IActionResult OnGet(string searchString)
        {
            Session = HttpContext.Session.GetInt32("Id");

            var shopid = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)Session).Result.Id;
            Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(p=>p.ShopId == shopid && p.IsRemoved==false).ToList();
       
            CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {

                Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(s => s.Name.Trim().ToLower().Contains(searchString.Trim().ToLower()) && s.ShopId == shopid && s.IsRemoved == false).ToList();
               
            }
            return Page();
            
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            await _unitOfWork.ProductRepository.UpdateProductStatusAysnc(true,id);
            var shopid = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)Session).Result.Id;
            Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(p => p.ShopId == shopid).ToList();
            return RedirectToPage("/Shops/AllProduct");
        }
    }
}
