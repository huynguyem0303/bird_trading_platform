using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Shops.Shared
{
    public class CreateShopModel : PageModel
    {
       
        public async Task<IActionResult> OnPostCreateNewShop()
        {
            return RedirectToPage("/Shops/NewShop");
        }
    }
}
