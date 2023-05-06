using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;
using OrdersApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration
.GetConnectionString("Connection") ?? throw new InvalidOperationException("Connection string  not found.");
builder.Services.AddDbContext<OrdersDbContext>(options => options.UseSqlite(connectionString));


var app = builder.Build();

// Resets order table
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
