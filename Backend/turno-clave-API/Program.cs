using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Application.Services;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add Swagger/OpenAPI for UI exploration (classic Swagger UI)
// Use the project's OpenAPI helper
builder.Services.AddOpenApi();

// Services and Repositories
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IProfessionalService, ProfessionalService>();
builder.Services.AddScoped<IProfessionalRepository, ProfessionalRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

// Register AppDbContext using PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseExceptionHandler("/error");
app.Map("/error", async (HttpContext httpContext) =>
{
    var problemDetails = new ProblemDetails
    {
        Type = "/errors/UnknownError", // Custom error type
        Title = "An unexpected error occurred.",
        Status = (int)HttpStatusCode.InternalServerError,
        Detail = "Something went wrong. Please try again later.",
        Instance = httpContext.Request.Path // Identifies where the error occurred
    };

    httpContext.Response.ContentType = "application/json";
    httpContext.Response.StatusCode = problemDetails.Status.Value;
    await httpContext.Response.WriteAsJsonAsync(problemDetails);
});

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

DateTimeOffset exampleDateTimeOffset = new(2026, 3, 1, 18, 0, 0, TimeSpan.FromHours(-3)); // March 1, 2026, at 18:00:00 with a -3 hours offset (e.g., Buenos Aires time)
Debug.WriteLine(exampleDateTimeOffset); // OUTPUT: 1/3/2026 18:00:00 -03:00

app.Run();
