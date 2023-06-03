using BirdTrading.DataAccess;
using BirdTrading.Interface;
using BirdTrading.Interface.Repositories;

namespace BirdTrading.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private readonly ICategoryRepository categoryRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository ordersDetailRepository;
        private readonly IProductRepository productRepository;
        private readonly IShippingInformationRepository shippingInformationRepository;
        private readonly IShippingSessionRepository shippingSessionRepository;
        private readonly IShopRepository shopRepository;
        private readonly IUserRepository userRepository;

        public UnitOfWork(AppDbContext context, ICategoryRepository categoryRepository,
            IOrderRepository orderRepository, IOrderDetailRepository ordersDetailRepository,
            IProductRepository productRepository, IShippingInformationRepository shippingInformationRepository,
            IShippingSessionRepository shippingSessionRepository, IShopRepository shopRepository,
            IUserRepository userRepository)
        {
            this.context = context;
            this.categoryRepository = categoryRepository;
            this.orderRepository = orderRepository;
            this.ordersDetailRepository = ordersDetailRepository;
            this.productRepository = productRepository;
            this.shippingInformationRepository = shippingInformationRepository;
            this.shippingSessionRepository = shippingSessionRepository;
            this.shopRepository = shopRepository;
            this.userRepository = userRepository;
        }

        public ICategoryRepository CategoryRepository => categoryRepository;
        public IOrderDetailRepository OrderDetailRepository => ordersDetailRepository;
        public IOrderRepository OrderRepository => orderRepository;
        public IProductRepository ProductRepository => productRepository;
        public IShippingInformationRepository ShippingInformationRepository => shippingInformationRepository;
        public IShippingSessionRepository ShippingSessionRepository => shippingSessionRepository;
        public IShopRepository ShopRepository => ShopRepository;

        public IUserRepository UserRepository => UserRepository;

        public async Task<bool> SaveChangeAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
