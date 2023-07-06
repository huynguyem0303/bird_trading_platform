using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Shops.Shared
{
    public class CreateShopModel : PageModel
    {
        public int? Session { get; set; }
        public async Task<IActionResult> OnPostCreateNewShop()
        {
            Session = HttpContext.Session.GetInt32("Id");
            return RedirectToPage("/Shops/NewShop");
        }
    }
}
