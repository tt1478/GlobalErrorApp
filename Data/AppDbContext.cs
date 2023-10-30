using GlobalErrorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalErrorApp.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<Driver> Drivers { get; set; }
    }
}
