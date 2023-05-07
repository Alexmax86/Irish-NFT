using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NFTmvc.Areas.Identity.Data;
using System.Net.Http.Headers;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("NFTmvcIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'NFTmvcIdentityDbContextConnection' not found.");

builder.Services.AddDbContext<NFTmvcIdentityDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<NFTmvcIdentityDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adds HttoClient for Products API
builder.Services.AddHttpClient("ProductsApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5142/");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(
        mediaType: "application/json",
        quality: 1.0
        )
    );
});

//Adds HttoClient for Orders API
builder.Services.AddHttpClient("OrdersApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5050/");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(
        mediaType: "application/json",
        quality: 1.0
        )
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


app.Run();
