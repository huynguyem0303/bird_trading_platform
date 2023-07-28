using BirdTrading.Domain.Enums;
using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BirdTradingApp.Pages.Shops
{
    public class CompetedOrderModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompetedOrderModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public SelectList Options { get; set; }
        public List<Order> Order { get; set; }
        public List<ShippingSession> ShippingSession { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public string CurrentFilter { get; set; }
        public bool checkNull = false;
        public int? Session { get; set; }
        public async Task OnGetAsync(string searchString, string searchBy)
        {
            Session = HttpContext.Session.GetInt32("Id");
            bool validate = false;
            bool checkcancel = false;
            var shopid = _unitOfWork.ShopRepository.GetShopsUserIdAysnc((int)Session).Result.Id;
            var productlist = _unitOfWork.ProductRepository.GetByShopIdAsync((int)shopid);
            foreach (var item in productlist.Result)
            {
                if (OrderDetail == null)
                {
                    OrderDetail = _unitOfWork.OrderDetailRepository.GetByProductIdAsync(item.Id).Result;
                    continue;
                }
                if (OrderDetail != null)
                {
                    OrderDetail.AddRange(_unitOfWork.OrderDetailRepository.GetByProductIdAsync(item.Id).Result);
                    continue;
                }
            }
            //foreach (var item in OrderDetail)
            //{
            //    if (ShippingSession == null)
            //    {
            //        ShippingSession = _unitOfWork.ShippingSessionRepository.GetByOrderIdAndStatusAsync(item.OrderId, OrderStatus.Delivered).Result;
            //        continue;
            //    }
            //    if (ShippingSession != null)
            //    {
            //        ShippingSession.AddRange(_unitOfWork.ShippingSessionRepository.GetByOrderIdAndStatusAsync(item.OrderId, OrderStatus.Delivered).Result);
            //        continue;
            //    }

            //}
            if (OrderDetail == null)
            {
                checkNull = true;

            }
            else
            {
                foreach (var item in OrderDetail)
                {
                    checkcancel = _unitOfWork.ShippingSessionRepository.CheckStatus(item.OrderId, OrderStatus.Cancel);
                    if (!checkcancel)
                    {
                        if (Order == null)
                        {
                            Order = _unitOfWork.OrderRepository.GetByOrderDetailIdAsync(item.OrderId).Result;
                            continue;
                        }
                        if (Order != null)
                        {
                            Order.AddRange(_unitOfWork.OrderRepository.GetByOrderDetailIdAsync(item.OrderId).Result);
                            continue;

                        }
                    }
                }
                if (Order == null)
                {
                    checkNull = true;
                }
            }



        }
    }
}
