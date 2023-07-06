using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BirdTradingApp.Pages.Shops
{
    public class OutOfStockModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public OutOfStockModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public SelectList Options { get; set; }
        public List<Product> Product { get; set; }
        public string CurrentFilter { get; set; }

        public int? Session { get; set; }
        public async Task OnGetAsync(string searchString, string searchBy)
        {
            var currentUserLoginId = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user").Id;
            var shopid = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)currentUserLoginId).Result.Id;
            Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(p => p.ShopId == shopid && p.Quantity==0 && p.IsRemoved == false).ToList();

            CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {

                Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(s => s.Name.Trim().ToLower().Contains(searchString.Trim().ToLower())&& s.Quantity == 0 && s.IsRemoved == false).ToList();

            }

        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            await _unitOfWork.ProductRepository.UpdateProductStatusAysnc(true, id);
            var currentUserLoginId = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user").Id;
            var shopid = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)currentUserLoginId).Result.Id;
            Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(p => p.ShopId == shopid).ToList();
            return RedirectToPage("/Shops/OutOfStock");
        }
    }
}
