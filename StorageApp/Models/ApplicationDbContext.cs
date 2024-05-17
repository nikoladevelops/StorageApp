using Microsoft.EntityFrameworkCore;

namespace StorageApp.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}
