using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Admin
{
    public class DashBoardModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        public int TotalUser {get; set;}
        public int TotalShop { get; set;}
        public int TotalOrder { get; set;}
        public decimal TotalIncome { get; set;}
        public int[] UserStatic {  get; set;}
        public int[] ShopStatic { get; set;}
        public decimal[] OrderStatic { get; set;}   
        public DashBoardModel(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }
        public IActionResult OnGet()
        {
            string role = _contextAccessor.HttpContext.Session.GetString("Role");
            if (role is null || !role.Equals("Admin"))
            {
                return RedirectToPage("/Login/Index");
            }
            TotalUser = _unitOfWork.UserRepository.GetAllUsersExceptAdmin().Count();
            TotalShop = _unitOfWork.ShopRepository.GetAll().Count();
            TotalOrder = _unitOfWork.OrderRepository.GetAll().Count();
            TotalIncome = _unitOfWork.OrderRepository.GetAll().Sum(x => x.Total);
            UserStatic = new int[2];
            UserStatic[0] = _unitOfWork.UserRepository.DoStatic()[0];
            UserStatic[1] = _unitOfWork.UserRepository.DoStatic()[1];
            ShopStatic = new int[2];
            ShopStatic[0] = _unitOfWork.ShopRepository.GetAll().FindAll(x => x.IsBlocked == false).Count;
            ShopStatic[1] = _unitOfWork.ShopRepository.GetAll().FindAll(x => x.IsBlocked == true).Count;
            OrderStatic = new decimal[7];
            for(int i = 0; i < 7; i++)
            {
                OrderStatic[i] = _unitOfWork.OrderRepository.DoStatics()[i];
            }
            return Page();
        }
    }
}
