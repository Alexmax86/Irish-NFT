using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;

namespace OrdersApi.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        Console.WriteLine("Seeding data...");
        using (var context = new OrdersDbContext(
        serviceProvider.GetRequiredService<
        DbContextOptions<OrdersDbContext>>()))
        {
            //Resets products table every time API starts
            var entities = context.Orders.ToList();
            context.Orders.RemoveRange(entities);   
            context.SaveChanges();

            context.Database.ExecuteSqlRaw("UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Orders';");         

            

        }
    }
}