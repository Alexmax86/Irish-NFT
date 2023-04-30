using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;

namespace ProductsApi.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApiDbContext(
        serviceProvider.GetRequiredService<
        DbContextOptions<ApiDbContext>>()))
        {
            //Resets products table every time API starts
            var entities = context.Products.ToList();
            context.Products.RemoveRange(entities);   
            context.SaveChanges(); 
            /*
            if (context.Products.Any())
            {
                return; // DB has been seeded
            }
            */

            context.Products.AddRange(
                new Product{ Name = "Camera", Description = "Fujinon camera on black background", CreatedBy = "Lucas Hoang", Category = "Objects", Cost = 23, ImgLink = "http://localhost:5062/images/lucas-hoang-655I5rcZiPM-unsplash.jpg"},
                new Product{ Name = "Clown Fish", Description = "A clown fish spreading its fins", CreatedBy = "Matheen Faiz", Category = "Nature", Cost = 50, ImgLink = "http://localhost:5062/Images/matheen-faiz-H2ZfJhPWITY-unsplash.jpg"},
                new Product{ Name = "Sea view", Description = "A view on the sea", CreatedBy = "Tahir-Osman", Category = "Scenery", Cost = 50, ImgLink = "http://localhost:5062/Images/tahir-osman-SewnR4JAX9E-unsplash.jpg"},
                new Product{ Name = "Girl with a hat", Description = "Girl in '900 apparel holding flowers", CreatedBy = "Anita Austvika", Category = "People", Cost = 75, ImgLink = "http://localhost:5062/Images/anita-austvika-ys5M71E58eE-unsplash.jpg"},
                new Product{ Name = "Church", Description = "A black church on a field", CreatedBy = "David Becker", Category = "Building", Cost = 32, ImgLink = "http://localhost:5062/Images/david-becker-ItetJnX4gFY-unsplash.jpg"},
                new Product{ Name = "Quartz", Description = "Pink quartz on a table", CreatedBy = "Edz Norton", Category = "Nature", Cost = 25, ImgLink = "http://localhost:5062/Images/edz-norton-6kBh0dR07ZU-unsplash.jpg"},
                new Product{ Name = "Subaru desert", Description = "A Subaru rolling on dunes", CreatedBy = "Gustavo Zambelli", Category = "Cars", Cost = 150, ImgLink = "http://localhost:5062/Images/gustavo-zambelli-cEbHT8Vbb9g-unsplash.jpg"},
                new Product{ Name = "Surf board", Description = "A surf board on Turquoise background", CreatedBy = "Joel Severino ", Category = "Misc", Cost = 120, ImgLink = "http://localhost:5062/Images/joel-severino-Gd6cfMXh2Bc-unsplash.jpg"},
                new Product{ Name = "Daisies and Lilacs", Description = "A girls smelling flowers on a hill", CreatedBy = "Josh Hild", Category = "People", Cost = 100, ImgLink = "http://localhost:5062/Images/josh-hild-GDWle6useeg-unsplash.jpg"},
                new Product{ Name = "Venice", Description = "A shot of a Venice alley", CreatedBy = "Joshua Kettle", Category = "Urban", Cost = 90, ImgLink = "http://localhost:5062/Images/joshua-kettle-xxEEaDEBq9k-unsplash.jpg"},
                new Product{ Name = "Bison", Description = "Close-up of a bison", CreatedBy = "Mason Field", Category = "Nature", Cost = 90, ImgLink = "http://localhost:5062/Images/mason-field-KNfOAvrFtCM-unsplash.jpg"},
                new Product{ Name = "Church aisle", Description = "The main aisle of a medieval church", CreatedBy = "Rafael Martins", Category = "Building", Cost = 90, ImgLink = "http://localhost:5062/Images/rafael-as-martins-InDbiYuz70Y-unsplash.jpg"},
                new Product{ Name = "Heron", Description = "A heron flying on black background", CreatedBy = "Vincent Van Zalinge", Category = "Nature", Cost = 200, ImgLink = "http://localhost:5062/Images/vincent-van-zalinge-10SQWgx9Mr8-unsplash.jpg"}                
            );
            context.SaveChanges();

        }
    }
}