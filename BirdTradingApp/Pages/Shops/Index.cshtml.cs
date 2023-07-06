using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Shops
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId is not null)
            {
                var shop=_unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)userId);
                if (shop.Result == null) {
                    return RedirectToPage("/Shops/CreateShop");
                }
                return Page();
            }
            return RedirectToPage("/Login/Index");
        }
    }
}
