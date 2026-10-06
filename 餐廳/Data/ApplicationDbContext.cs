using Microsoft.EntityFrameworkCore;
using 餐廳.Models;

namespace 餐廳.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
        public DbSet<SupplyRecord> SupplyRecords { get; set; } = null!;
    }
}