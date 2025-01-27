using Microsoft.EntityFrameworkCore;
using OZON_Delivery_checker.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<TrackingAPIService>();
builder.Services.AddScoped<TrackingAPIService>();
builder.Services.AddScoped<TrackingDBService>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=DeliveryChecker.db"));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
