using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Product? Product { get; set; }
        public IEnumerable<Product> OtherProducts { get; set; }

        public async Task OnGet(int id)
        {
            Product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            OtherProducts = await _unitOfWork.ProductRepository.GetTop4RelateProductAsync(Product.Category.TypeId, Product.Id);
        }
    }
}
