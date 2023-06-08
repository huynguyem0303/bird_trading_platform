using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Utils.Pagination;
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
        public Pagination<Product> ProductPaging { get; set; }

        public async Task OnGetAsync(int pageIndex = 0, int pageSize = 9)
        {
            CategoryTypes = await _unitOfWork.CategoryTypeRepository.GetListAsync();
            ProductPaging = await _unitOfWork.ProductRepository.GetPaginationsAsync(pageIndex, pageSize);
        }

        public async Task OnGetSearchAsync(string search, int pageIndex = 0, int pageSize = 9)
        {
            if (search is null)
            {
                await OnGetAsync(pageIndex, pageSize);
            }
            else
            {
                CategoryTypes = await _unitOfWork.CategoryTypeRepository.GetListAsync();
                ProductPaging = await _unitOfWork.ProductRepository.SearchProductPagingAsync(search, pageIndex, pageSize);
            }
        }

        public async Task OnGetFilterAsync(string category, int pageIndex = 0, int pageSize = 9)
        {
            if (category is null)
            {
                await OnGetAsync(pageIndex, pageSize);
            }
            else
            {
                CategoryTypes = await _unitOfWork.CategoryTypeRepository.GetListAsync();
                ProductPaging = await _unitOfWork.ProductRepository.SearchProductPagingAsync(category, pageIndex, pageSize);
            }
        }
    }
}
