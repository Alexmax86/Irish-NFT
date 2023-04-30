using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;
using ProductsApi.Data;




var builder = WebApplication.CreateBuilder(args);

//Registering DB Context
var connectionString = builder.Configuration
.GetConnectionString("Connection") ??
throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<ApiDbContext>(options =>
options.UseSqlite(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
var services = scope.ServiceProvider;
SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
