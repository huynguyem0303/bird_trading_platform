using BirdTrading.Domain.Enums;
using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Shops
{
    public class OrderDetailModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<OrderDetail> OrderDetail { get; set; }
        public ShippingSession ShippingSession { get; set; }
        public int? Session { get; set; }
        public int orderid { get; set; }
        public bool check { get; set; }
        public bool check1{ get; set; }
        public bool check2 { get; set; }
        public bool check3 { get; set; }
        public bool check4 { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            if (id == null)
            {
                return NotFound();
            }

            OrderDetail = _unitOfWork.OrderDetailRepository.GetByOrderIdAsync((int)id).Result;

            if (OrderDetail == null)
            {
                return NotFound();
            }
            orderid = (int)id;
            check = _unitOfWork.ShippingSessionRepository.CheckStatus((int)id, OrderStatus.WaitingForConfirm);
            check1 = _unitOfWork.ShippingSessionRepository.CheckStatus((int)id, OrderStatus.WaitingForDelivery);
            check2 = _unitOfWork.ShippingSessionRepository.CheckStatus((int)id, OrderStatus.OnDelelivering);
            check3 = _unitOfWork.ShippingSessionRepository.CheckStatus((int)id, OrderStatus.Delivered);
            check4 = _unitOfWork.ShippingSessionRepository.CheckStatus((int)id, OrderStatus.Cancel);
            return Page();
        }
        public async Task<IActionResult> OnPostConfirm(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            var shippingsession = new ShippingSession();
            shippingsession.OrderId = id;
            shippingsession.SessionDate = DateTime.Now;
            shippingsession.Status = OrderStatus.WaitingForDelivery;
            shippingsession.Description = "Waiting For Delivery";
  
            _unitOfWork.ShippingSessionRepository.CreateSessionAysnc(shippingsession);
            return RedirectToPage("/Shops/WaitingConfirm");
        }
        public async Task<IActionResult> OnPostOnDelivering(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            var shippingsession = new ShippingSession();
            shippingsession.OrderId = id;
            shippingsession.SessionDate = DateTime.Now;
            shippingsession.Status = OrderStatus.OnDelelivering;
            shippingsession.Description = "On Delivering";

            _unitOfWork.ShippingSessionRepository.CreateSessionAysnc(shippingsession);
            return RedirectToPage("/Shops/WaitingForDelelivering");
        }
        public async Task<IActionResult> OnPostDelivered(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            var shippingsession = new ShippingSession();
            shippingsession.OrderId = id;
            shippingsession.SessionDate = DateTime.Now;
            shippingsession.Status = OrderStatus.Delivered;
            shippingsession.Description = "Delivered";

            _unitOfWork.ShippingSessionRepository.CreateSessionAysnc(shippingsession);
            return RedirectToPage("/Shops/OnDelivering");
        }
        public async Task<IActionResult> OnPostCancel(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            var shippingsession = new ShippingSession();
            shippingsession.OrderId = id;
            shippingsession.SessionDate = DateTime.Now;
            shippingsession.Status = OrderStatus.Cancel;
            shippingsession.Description = "Canceled";

            _unitOfWork.ShippingSessionRepository.CreateSessionAysnc(shippingsession);
            return RedirectToPage("/Shops/CancelOrder");
        }
     
    }
}
