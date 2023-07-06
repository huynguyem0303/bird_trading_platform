using BirdTrading.Domain.Enums;
using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTradingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BirdTradingApp.Pages.Shops
{
    public class CheckHistoryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public CheckHistoryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public SelectList Options { get; set; }
        public List<Order> Order { get; set; }
        public List<ShippingSession> ShippingSession { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public string CurrentFilter { get; set; }
        public bool checkNull = false;
        public int? orderid { get; set; }
        public int? Session { get; set; }
        public IActionResult OnGetAsync(int id)
        {
            Session = HttpContext.Session.GetInt32("Id");
            ShippingSession =_unitOfWork.ShippingSessionRepository.CheckHistory(id).Result;
            orderid = id;
            return Page();
        }
    }
}
