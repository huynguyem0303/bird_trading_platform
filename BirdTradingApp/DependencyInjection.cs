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
        }

        public static void InjectService(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddRazorPages();
        }
    }
}
