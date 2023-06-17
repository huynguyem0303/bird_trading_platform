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

        public Product Product { get; set; }
        public IEnumerable<Product> OtherProducts { get; set; }
        public float AverageRating { get; set; }
        public int NumOfRating { get; set; }

        public async Task OnGet(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product is not null)
            {
                Product = product;
                OtherProducts = await _unitOfWork.ProductRepository.GetTop8RelateProductAsync(Product.Category.TypeId, Product.Id);
                foreach (var item in product.OrderDetails)
                {
                    AverageRating += item.Rating ?? 0;
                }
                NumOfRating = product.OrderDetails.Where(x => x.Rating != null).Count();
                AverageRating /= NumOfRating;
                AverageRating = (float)Math.Floor(AverageRating) + 0.5f;
            }
        }
    }
}
