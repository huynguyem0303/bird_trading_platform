using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CategoryType> CategoryTypes { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public async Task OnGet()
        {
            Products = await _unitOfWork.ProductRepository.GetListAsync();
            CategoryTypes = await _unitOfWork.CategoryTypeRepository.GetListAsync();
        }
    }
}
