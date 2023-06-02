using Microsoft.EntityFrameworkCore;

namespace BirdTrading.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
