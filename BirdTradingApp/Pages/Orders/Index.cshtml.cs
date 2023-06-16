using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;

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
                return Partial("OrdersPartials/_CartListPartial", Carts);
            }
            return BadRequest("Product not found");
        }
        #endregion

        #region UpdateQuantity
        public async Task<IActionResult> OnGetUpdateQuantityAsync(int detailId, int quantity)
        {
            var details = await _unitOfWork.CartDetailRepository.GetByIdAsync(detailId);
            if (details is null) return BadRequest("Product not found in cart.");
            //
            if (quantity > 0 && details.Product.Quantity >= quantity)
            {
                details.Quantity = quantity;
                _unitOfWork.CartDetailRepository.Update(details);
                if (await _unitOfWork.SaveChangeAsync()) return new JsonResult( new
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Update succeed",
                    ProductPrice = details.Product.OriginalPrice,
                    Quantity = quantity
                });
            }

            return new JsonResult(new
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Product quantity is not enough or invalid.",
            });
        }
        #endregion
        //
        public int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("Id") ?? -1;
        }
    }
}
