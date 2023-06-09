using BirdTrading.DataAccess;
using BirdTrading.Interface;
using BirdTrading.Interface.Repositories;
using BirdTrading.Repository;
using BirdTrading.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BirdTradingApp
{
    public static class DependencyInjection
    {
        public static void InjectInfracstucture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("AppDb")));
            //
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<ICartDetailRepository, CartDetailRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICategoryTypeRepository, CategoryTypeRepository>();
            services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IShippingInformationRepository, ShippingInformationRepository>();
            services.AddTransient<IShippingSessionRepository, ShippingSessionRepository>();
            services.AddTransient<IShopRepository, ShopRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        public static void InjectService(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //
            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromMinutes(60);
            });
        }
    }
}
