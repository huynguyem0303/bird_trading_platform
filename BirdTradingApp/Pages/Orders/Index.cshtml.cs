using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Cart> Carts { get; set; }


        public async Task OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId is not null)
            {
                Carts = await _unitOfWork.CartRepository.GetUserCartAsync((int)userId);
            }
        }

        #region RemoveFromCartHandle
        public async Task OnGetRemoveAsync(int detailsId)
        {
            var details = await _unitOfWork.CartDetailRepository.GetByIdAsync(detailsId);
            if (details is null)
            {
                await OnGetAsync();
                return;
            }
            _unitOfWork.CartDetailRepository.Delete(details);
            if (await _unitOfWork.SaveChangeAsync())
            {
                TempData["success"] = $"Product #{details.ProductId} Removed";
            }
            await OnGetAsync();
        }
        #endregion
    }
}
