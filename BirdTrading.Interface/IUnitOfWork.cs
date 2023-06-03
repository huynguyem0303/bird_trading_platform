using BirdTrading.Interface.Repositories;

namespace BirdTrading.Interface
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRepository ProductRepository { get; }
        IShippingInformationRepository ShippingInformationRepository { get; }
        IShippingSessionRepository ShippingSessionRepository { get; }
        IShopRepository ShopRepository { get; }
        IUserRepository UserRepository { get; }
        Task<bool> SaveChangeAsync();
    }
}
