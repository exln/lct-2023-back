using MediWingWebAPI.Data;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApiDbContext>(options => options
    .UseNpgsql(connectionString, b => b.MigrationsAssembly("MediWingWebAPI")));

builder.Services.AddCors(); // Add CORS support here

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

app.UseHttpsRedirection();
app.UseRouting(); // Add UseRouting before UseCors 
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().WithExposedHeaders("content-disposition").WithHeaders("content-type"));; // Allow all origins and methods for CORS 
app.UsePathBase(pathBase: "/api"); // Add UsePathBase here

app.UseAuthorization();

app.MapControllers();

app.Run();