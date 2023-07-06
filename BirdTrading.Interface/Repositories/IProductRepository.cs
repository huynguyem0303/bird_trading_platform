using BirdTrading.Domain.Models;
using BirdTrading.Utils.Pagination;

namespace BirdTrading.Interface.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Pagination<Product>> SearchProductPagingAsync(string search, int pageIndex, int pageSize);
        Task<IEnumerable<Product>> GetTop8RelateProductAsync(int categoryType, int productId);
        Task<Pagination<Product>> GetProductPagingByCategoryTypeAsync(int categoryType, int pageIndex, int pageSize);
        Task<Pagination<Product>> GetProductPagingByCategoryAsync(int category, int pageIndex, int pageSize);
        List<Product> GetProductByShopId(int shopId);
    }
}
