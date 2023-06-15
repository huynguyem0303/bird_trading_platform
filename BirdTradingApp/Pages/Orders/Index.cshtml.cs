using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc;
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
            var userId = GetCurrentUserId();
            if (userId > 0) Carts = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
        }

        #region RemoveFromCartHandle
        public async Task<IActionResult> OnGetRemoveAsync(int detailsId)
        {
            var details = await _unitOfWork.CartDetailRepository.GetByIdAsync(detailsId);
            if (details is null) return BadRequest("Product not found");
            //
            _unitOfWork.CartDetailRepository.Delete(details);
            if (await _unitOfWork.SaveChangeAsync())
            {
                var cart = await _unitOfWork.CartRepository.GetByIdAsync(details.CartId);
                if (cart is not null && cart.CartDetails.Count() == 0)
                {
                    _unitOfWork.CartRepository.Delete(cart);
                    await _unitOfWork.SaveChangeAsync();
                }
                TempData["success"] = $"Product {details.Product.Name} removed from cart";
                var userId = GetCurrentUserId();
                if (userId > 0) Carts = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
                return Partial("OrdersAjax/_CartListPartial", Carts);
            }
            return BadRequest("Product not found");
        }
        #endregion
        //
        public int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("Id") ?? -1;
        }
    }
}
