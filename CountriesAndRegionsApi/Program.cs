using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.ModelContext;
using Application.Services.Interfaces;
using Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json file
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.AddDbContext<CountryContext>((options, context) =>
{
    // Get connection string from configuration
    string connectionString = builder.Configuration.GetConnectionString("DbContext");

    // Configure DbContext options
    context.UseSqlServer(connectionString);
});

// Add services to the container.
builder.Services.AddControllers();

//builder.Services.AddDbContext<CountryContext>();
builder.Services.AddScoped<ICountryService, CountryService>();

builder.Services.AddMemoryCache();

builder.Services.AddLogging(x => x.AddConsole());


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
