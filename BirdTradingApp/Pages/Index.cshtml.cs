using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CategoryType> CategoryTypes { get; set; }
        private IEnumerable<Product> Products { get; set; }
        public IEnumerable<Product> RecentProducts { get; set; }
        public IEnumerable<Product> FeatureProducts { get; set; } = new List<Product>();
        public IEnumerable<Shop> Shops { get; set; }

        public async Task OnGetAsync()
        {
            CategoryTypes = await _unitOfWork.CategoryTypeRepository.GetListAsync();
            Products = await _unitOfWork.ProductRepository.GetListAsync();
            Shops = await _unitOfWork.ShopRepository.GetListAsync();
            RecentProducts = Products.OrderByDescending(x => x.Id)
                .Take(8).ToList();
            var featureProductsId = await _unitOfWork.OrderDetailRepository.GetTop8PopularProducts();
            var featureProducts = new List<Product>();
            foreach (var item in featureProductsId)
            {
                featureProducts.Add(Products.First(x => x.Id == item));
            }
            FeatureProducts = featureProducts;
        }
        //
        #region Extend Function
        public async Task<float> GetAverageRating(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            var result = 0f;
            if (product is not null)
            {
                foreach (var item in product.OrderDetails)
                {
                    result += item.Rating ?? 0;
                }
                var numOfRating = product.OrderDetails.Where(x => x.Rating != null).Count();
                result /= numOfRating;
                result = (float)Math.Floor(result) + 0.5f;
            }
            return result;
        }

        public async Task<int> GetNumberOfRating(int productId)
        {
            var result = 0;

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product is not null)
            {
                result = product.OrderDetails.Where(x => x.Rating != null).Count();
            }
            return result;
        }
        #endregion
    }
}