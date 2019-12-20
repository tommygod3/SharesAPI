using SharesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SharesAPI.DatabaseAccess
{
    public class AppDbContext : DbContext
    {
        //passes options to overloaded base implementation
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
