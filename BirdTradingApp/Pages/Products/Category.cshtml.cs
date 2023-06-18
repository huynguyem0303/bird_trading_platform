using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Utils.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Products
{
    public class CategoryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<CategoryType> CategoryTypes { get; set; }
        public Pagination<Product> ProductPaging { get; set; }
        public Category RecentCategory { get; set; }
        public string RecentType { get; set; }
        public int CurrentPageIndex { get; set; }
        public int CurrentPageSize { get; set; }

        public async Task OnGetAsync(int categoryType, int pageIndex = 0, int pageSize = 9)
        {
            CategoryTypes = await _unitOfWork.CategoryTypeRepository.GetListAsync();
            Categories = await _unitOfWork.CategoryRepository.GetListByTypeIdAsync(categoryType);
            ProductPaging = await _unitOfWork.ProductRepository.GetProductPagingByCategoryTypeAsync(categoryType, pageIndex, pageSize);
            var type = await _unitOfWork.CategoryTypeRepository.GetByIdAsync(categoryType);
            if (type is not null) RecentType = type.Type;
            CurrentPageIndex = pageIndex;
            CurrentPageSize = pageSize;
        }

        public async Task OnGetByCategoryAsync(int category, int pageIndex = 0, int pageSize = 9)
        {
            ProductPaging = await _unitOfWork.ProductRepository.GetProductPagingByCategoryAsync(category, pageIndex, pageSize);
            var categoryObj = await _unitOfWork.CategoryRepository.GetByIdAsync(category);
            if (categoryObj is not null) RecentCategory = categoryObj;
            CategoryTypes = await _unitOfWork.CategoryTypeRepository.GetListAsync();
            RecentType = RecentCategory.Type.Type;
            CurrentPageIndex = pageIndex;
            CurrentPageSize = pageSize;
        }
        //
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
    }
}
