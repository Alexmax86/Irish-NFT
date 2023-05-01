using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;
namespace OrdersApi.Data
{
public class OrdersDbContext : DbContext
{
public OrdersDbContext(DbContextOptions<OrdersDbContext> options) :
base(options)
{}
public DbSet<Order> Orders { get; set; } = null!;
}
}
