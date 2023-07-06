using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Shops
{
    public class ShippingInformationModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShippingInformationModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      
        public ShippingInformation ShippingInformation { get; set; }
        public int? Session { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Session = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return NotFound();
            }

            ShippingInformation = _unitOfWork.ShippingInformationRepository.GetShippingInformationAsync((int)id).Result;

            if (ShippingInformation == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
