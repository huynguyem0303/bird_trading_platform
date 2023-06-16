using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Order Details { get; set; }

        public async Task OnGet(int detailId)
        {
            var detail = await _unitOfWork.OrderRepository.GetByIdAsync(detailId);
            if (detail is not null) Details = detail;
        }
    }
}
