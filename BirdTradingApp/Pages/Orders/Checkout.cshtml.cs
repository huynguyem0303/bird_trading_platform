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
        public IEnumerable<ShippingInformation> AddressList { get; set; }

        public async Task OnGet(string cartDetailsId)
        {
            if (cartDetailsId is null) return;
            //
            CartDetails = await GenerateDetailListAsync(cartDetailsId);
            AddressList = await GetUserShippingInformationAsync();
            if (AddressList.Count() > 0) ShippingInformation = AddressList.First(x => x.IsDefaultAddress)!;
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

        public async Task<IActionResult> OnGetPaymentAsync(string cartDetailsId, int addressId)
        {
            if (string.IsNullOrEmpty(cartDetailsId) || !await IsExistAddressAsync(addressId)) return RedirectToPage("Index");
            var cartDetails = await GenerateDetailListAsync(cartDetailsId);
            var orderList = GenerateListOrder(cartDetails, addressId);
            await UpdateOrderAsync(orderList);
            await RemoveCartAsync(cartDetails);
            await UpdateProductQuantity(cartDetails);
            TempData["success"] = "Succeed";
            return RedirectToPage("Index");
        }

        #region ChangeAddress
        public async Task OnGetChangeAddressAsync(int addressId, string cartDetailsId)
        {
            if (!await IsExistAddressAsync(addressId))
            {
                TempData["error"] = "Address is invalid";
                await OnGet(cartDetailsId);
                return;
            }
            //
            var address = await _unitOfWork.ShippingInformationRepository.GetByIdAsync(addressId);
            if (address is null)
            {
                TempData["error"] = "Address doesn't exists";
                await OnGet(cartDetailsId);
                return;
            }
            //
            await OnGet(cartDetailsId);
            ShippingInformation = address;
        }
        #endregion

        //
        public int GetCurrentUserId()
        {
            var id = HttpContext.Session.GetInt32("Id") ?? -1;
            return id;
        }

        public async Task<bool> IsExistAddressAsync(int addressId)
        {
            var address = await _unitOfWork.ShippingInformationRepository.GetByIdAsync(addressId);
            return address is not null;
        }

        #region Checkout Func
        public async Task<IEnumerable<ShippingInformation>> GetUserShippingInformationAsync()
        {
            var id = GetCurrentUserId();
            return await _unitOfWork.ShippingInformationRepository.GetUserShippingInformationAsync(id);
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

        public IEnumerable<Order> GenerateListOrder(IEnumerable<CartDetail> detailCheckout, int addressId)
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
                var shippingStatus = new List<ShippingSession> { CreateShippingStatus() };
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    CompanyName = "GHN",
                    UserId = GetCurrentUserId(),
                    ShippingInformationId = addressId,
                    OrderDetails = orderDetails,
                    ShippingSessions = shippingStatus,
                    Total = total
                };
                //
                listOrder.Add(order);
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
            var cartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;
            if (cartCount != 0) HttpContext.Session.SetInt32("CartCount", cartCount - cartIds.Count());
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

        public async Task UpdateProductQuantity(IEnumerable<CartDetail> cartDetails)
        {
            var listProducts = new List<Product>();
            foreach (var item in cartDetails)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if (product is not null)
                {
                    product.Quantity -= item.Quantity;
                    listProducts.Add(product);
                }
            }
            _unitOfWork.ProductRepository.UpdateRange(listProducts);
            await _unitOfWork.SaveChangeAsync();
        }

        public ShippingSession CreateShippingStatus()
        {
            return new ShippingSession
            {
                Status = BirdTrading.Domain.Enums.OrderStatus.WaitingForConfirm,
                SessionDate = DateTime.Now,
            };
        }
        #endregion
    }
}