using SharesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SharesAPI.DatabaseAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
