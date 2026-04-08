using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Net;
using System.Text;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Application.Services;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register Swagger/OpenAPI generator and enable JWT Bearer support in the UI
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register AppDbContext using PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var key = builder.Configuration["Jwt:Key"];

// Validate JWT signing key length (HS256 requires at least 256 bits = 32 bytes)
if (string.IsNullOrWhiteSpace(key) || Encoding.UTF8.GetBytes(key).Length < 32)
{
    throw new InvalidOperationException("The JWT signing key is not configured or is too short. Set 'Jwt:Key' to at least 32 bytes (256 bits) in configuration.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key))
    };
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

DateTimeOffset exampleDateTimeOffset = new(2026, 3, 1, 18, 0, 0, TimeSpan.FromHours(-3)); // March 1, 2026, at 18:00:00 with a -3 hours offset (e.g., Buenos Aires time)
Debug.WriteLine(exampleDateTimeOffset); // OUTPUT: 1/3/2026 18:00:00 -03:00

app.Run();
