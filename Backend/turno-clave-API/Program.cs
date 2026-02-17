using Microsoft.EntityFrameworkCore;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Application.Services;
using turno_clave_API.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add Swagger/OpenAPI for UI exploration (classic Swagger UI)
// Use the project's OpenAPI helper
builder.Services.AddOpenApi();

builder.Services.AddScoped<IBusinessService, BusinessService>();

// Register AppDbContext using PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Map the project's OpenAPI endpoint(s)
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Turno Clave API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
