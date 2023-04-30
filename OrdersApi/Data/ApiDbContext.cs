using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;
namespace OrdersApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; } = null!;
    }
}