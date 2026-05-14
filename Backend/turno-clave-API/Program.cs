using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Net;
using System.Text;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Application.Services;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Prevent JSON serialization errors when EF navigation properties create cycles
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // To return enum string instead of number
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddAuthorization();

// Register Swagger/OpenAPI generator and enable JWT Bearer support in the UI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Turno Clave API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter token",
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    options.AddSecurityRequirement( document => 
    new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

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

builder.Services.AddScoped<IProfessionalAvailabilityService, ProfessionalAvailabilityService>();
builder.Services.AddScoped<IProfessionalAvailabilityRepository, ProfessionalAvailabilityRepository>();

builder.Services.AddScoped<IBusinessAvailabilityRepository, BusinessAvailabilityRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

string connectionString;

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}
else
{
    // In production, read the connection string from environment variable
    connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
        ?? throw new InvalidOperationException("Environment variable 'DATABASE_URL' is not set.");
}

// Register AppDbContext using PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

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

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Console.WriteLine($"PORT ENV: {Environment.GetEnvironmentVariable("PORT")}");

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

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
    // Enable Swagger middleware and UI in development
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Turno Clave API V1");
    });
}

//if (!app.Environment.IsDevelopment())
//{
//app.UseHttpsRedirection();
//}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

DateTimeOffset exampleDateTimeOffset = new(2026, 3, 1, 18, 0, 0, TimeSpan.FromHours(-3)); // March 1, 2026, at 18:00:00 with a -3 hours offset (e.g., Buenos Aires time)
Debug.WriteLine(exampleDateTimeOffset); // OUTPUT: 1/3/2026 18:00:00 -03:00

// Apply any pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
