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
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Shop> Shops { get; set; }

        public async Task OnGetAsync()
        {
            CategoryTypes = await _unitOfWork.CategoryTypeRepository.GetListAsync();
            Products = await _unitOfWork.ProductRepository.GetListAsync();
            Shops = await _unitOfWork.ShopRepository.GetListAsync();
        }
    }
}