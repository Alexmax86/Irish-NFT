using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;
namespace ProductsApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; } = null!;
    }
}