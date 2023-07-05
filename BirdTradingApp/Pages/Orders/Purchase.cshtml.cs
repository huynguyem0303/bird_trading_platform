using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.CustomAuthorize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Orders
{
    [UserAuthorize]
    public class PurchaseModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Order> Orders { get; set; }

        public async Task OnGetAsync(string type)
        {
            var userId = GetCurrentUserId();
            if (userId > 0)
            {
                if (string.IsNullOrEmpty(type))
                {
                    Orders = await _unitOfWork.OrderRepository.GetListByUserIdAsync(userId);
                }
                else
                {
                    var typeValue = int.Parse(type);
                    Orders = await _unitOfWork.OrderRepository.GetListByUserIdAndStatusAsync(userId, typeValue);
                }
            }
        }

        public async Task OnGetSearchAsync(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                await OnGetAsync(string.Empty);
                return;
            }
            //
            var userId = GetCurrentUserId();
            if (userId > 0)
            {
                Orders = await _unitOfWork.OrderRepository.GetListSearchAsync(userId, search);
            }
        }

        public async Task<IActionResult> OnPostRatingAsync(string comment, float rating, int orderId, string type)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order is not null)
            {
                foreach (var item in order.OrderDetails)
                {
                    item.Comment = comment;
                    item.Rating = rating;
                }
                _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.SaveChangeAsync();
                TempData["success"] = "Add comment succeed. Thanks for your rating.";
                return RedirectToPage("/Orders/Purchase", type);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync(int orderId, string reason)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order is not null)
            {
                ShippingSession session = new ShippingSession
                {
                    OrderId = orderId,
                    Description = "Buyer cancels order with reason: " + reason,
                    SessionDate = DateTime.Now,
                    Status = BirdTrading.Domain.Enums.OrderStatus.Cancel
                };

                await _unitOfWork.ShippingSessionRepository.AddAsync(session);
                if (await _unitOfWork.SaveChangeAsync())
                {
                    TempData["success"] = "Cancel order succeed.";
                    return RedirectToPage("/Orders/Purchase", new { type = 4 });
                }
            }

            return Page();
        }
        //
        public int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("Id") ?? -1;
        }
    }
}
