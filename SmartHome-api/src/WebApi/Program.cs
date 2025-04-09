using System.Diagnostics.CodeAnalysis;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositoryServices();
builder.Services.AddUtilsServices();
builder.Services.AddBusinessLogicServices();

builder.Services.AddResponseCaching();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});

var connectionString = builder.Configuration.GetConnectionString("SmartHome");
IServiceCollection addDbContext = builder.Services.AddDbContext<SmartHomeDBContext>(options =>
    options.UseSqlServer(connectionString));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCaching();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}
