using Microsoft.EntityFrameworkCore;

namespace AppDocker.API.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}
