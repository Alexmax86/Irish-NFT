using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;
using OrdersApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Registering DB Context
var connectionString = builder.Configuration.GetConnectionString("Connection") ??
throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlite(connectionString));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();