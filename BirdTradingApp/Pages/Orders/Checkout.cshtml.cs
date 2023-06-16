using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Orders
{
    public class CheckoutModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public decimal CurrentTotal { get; set; }
        public IEnumerable<CartDetail> CartDetails { get; set; }
        public ShippingInformation ShippingInformation { get; set; }

        public async Task OnGet(string cartDetailsId)
        {
            if (cartDetailsId is null) return;
            //
            CartDetails = await GenerateDetailListAsync(cartDetailsId);
            var shippingInformation = await GetShippingInformationAsync();
            if (shippingInformation is not null) ShippingInformation = shippingInformation;
        }

        //
        public async Task<IActionResult> OnGetTotalAsync(int cartDetailsId, string currentTotalStr, bool isChecked)
        {
            var details = await _unitOfWork.CartDetailRepository.GetByIdAsync(cartDetailsId);
            if (details is not null)
            {
                var currentTotal = decimal.Parse(currentTotalStr.Substring(1));
                if (isChecked)
                {
                    currentTotal += (details.Quantity * details.Product.OriginalPrice);
                    CurrentTotal = currentTotal;
                }
                else
                {
                    currentTotal -= (details.Quantity * details.Product.OriginalPrice);
                    CurrentTotal = currentTotal <= 0 ? 0 : currentTotal;
                }
            }

            return Partial("OrdersPartials/_CheckoutPartial", this);
        }

        public async Task<IActionResult> OnGetPaymentAsync(string cartDetailsId)
        {
            if (string.IsNullOrEmpty(cartDetailsId)) return RedirectToPage("Index");
            var cartDetails = await GenerateDetailListAsync(cartDetailsId);
            var orderList = await GenerateListOrderAsync(cartDetails);
            await UpdateOrderAsync(orderList);
            await RemoveCartAsync(cartDetails);
            TempData["success"] = "Succeed";
            return RedirectToPage("Index");
        }

        //
        public int GetCurrentUserId()
        {
            var id = HttpContext.Session.GetInt32("Id") ?? -1;
            return id;
        }

        public async Task<ShippingInformation?> GetShippingInformationAsync()
        {
            var id = GetCurrentUserId();
            var shippingInformation = await _unitOfWork.ShippingInformationRepository.GetDefaultShippingInformationAsync(id);
            return shippingInformation;
        }

        public async Task<IEnumerable<CartDetail>> GenerateDetailListAsync(string cartDetailsId)
        {
            var detailIdsArr = cartDetailsId.Split(';');
            var detailList = new List<CartDetail>();
            decimal currentTotal = 0;
            //
            foreach (var item in detailIdsArr.SkipLast(1))
            {
                var id = int.Parse(item);
                var cartDetail = await _unitOfWork.CartDetailRepository.GetByIdAsync(id);
                if (cartDetail is not null)
                {
                    detailList.Add(cartDetail);
                    currentTotal += (cartDetail.Product.OriginalPrice * cartDetail.Quantity);
                }
            }
            //
            CurrentTotal = currentTotal;
            return detailList;
        }

        public async Task<IEnumerable<Order>> GenerateListOrderAsync(IEnumerable<CartDetail> detailCheckout)
        {
            var numOfOrder = detailCheckout.Select(x => x.Product.ShopId).Distinct();
            var listOrder = new List<Order>();
            decimal total = 0;
            foreach (var item in numOfOrder)
            {
                var orderDetails = new List<OrderDetail>();
                foreach (var cartDetail in detailCheckout)
                {
                    if (cartDetail.Product.ShopId == item)
                    {
                        var detail = new OrderDetail
                        {
                            Quantity = cartDetail.Quantity,
                            Price = cartDetail.Product.OriginalPrice,
                            ProductId = cartDetail.ProductId,
                        };
                        orderDetails.Add(detail);
                    }
                    total += cartDetail.Product.OriginalPrice * cartDetail.Quantity;
                }
                //
                var shippingInfor = await GetShippingInformationAsync();
                var shippingStatus = new List<ShippingSession> { CreateShippingStatus() };
                if (shippingInfor is not null) {

                    var order = new Order
                    {
                        OrderDate = DateTime.Now,
                        CompanyName = "GHN",
                        UserId = GetCurrentUserId(),
                        ShippingInformationId = shippingInfor.Id,
                        OrderDetails = orderDetails,
                        ShippingSessions = shippingStatus,
                        Total = total
                    };
                    //1
                    listOrder.Add(order);
                }
            }

            return listOrder;
        }

        public async Task<bool> UpdateOrderAsync(IEnumerable<Order> orders)
        {
            await _unitOfWork.OrderRepository.AddRangeAsync(orders);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> RemoveCartAsync(IEnumerable<CartDetail> cartDetails)
        {
            var cartIds = cartDetails.Select(x => x.CartId).Distinct();
            _unitOfWork.CartDetailRepository.DeleteRange(cartDetails.ToList());
            //
            if (await _unitOfWork.SaveChangeAsync())
            {
                foreach (var item in cartIds)
                {
                    var cart = await _unitOfWork.CartRepository.GetByIdAsync(item);
                    if (cart is null) return false;
                    //
                    if (cart.CartDetails.Count() == 0) _unitOfWork.CartRepository.Delete(cart);
                }
                return await _unitOfWork.SaveChangeAsync();
            }
            return false;
        }

        public ShippingSession CreateShippingStatus()
        {
            return new ShippingSession
            {
                Status = BirdTrading.Domain.Enums.OrderStatus.WaitingForConfirm,
                SessionDate = DateTime.Now,
            };
        }
    }
}