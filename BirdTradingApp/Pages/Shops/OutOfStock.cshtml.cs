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
            Session = HttpContext.Session.GetInt32("Id");
         
            var shopid = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)Session).Result.Id;
            Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(p => p.ShopId == shopid && p.Quantity==0 && p.IsRemoved == false).OrderByDescending(p=>p.Id).ToList();

            CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {

                Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(s => s.Name.Trim().ToLower().Contains(searchString.Trim().ToLower())&& s.Quantity == 0 && s.IsRemoved == false && s.ShopId == shopid).ToList();

            }

        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            await _unitOfWork.ProductRepository.UpdateProductStatusAysnc(true, id);
           
            var shopid = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)Session).Result.Id;
            Product = _unitOfWork.ProductRepository.GetProductsListAsync().Result.Where(p => p.ShopId == shopid).ToList();
            return RedirectToPage("/Shops/OutOfStock");
        }
    }
}
