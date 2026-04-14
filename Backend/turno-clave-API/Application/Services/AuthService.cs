using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly IUserService _userService;

    public AuthService(AppDbContext context, IConfiguration config, IUserService userService)
    {
        _context = context;
        _config = config;
        _userService = userService;
    }

    public async Task<string> ValidateGoogle(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            throw new ArgumentException("idToken is required", nameof(idToken));

        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken)
            ?? throw new InvalidOperationException("Failed to validate Google id token.");

        if (string.IsNullOrWhiteSpace(payload.Subject))
            throw new InvalidOperationException("Google payload subject is missing.");

        User? user = _context.Users
            .FirstOrDefault(u => u.GoogleId == payload.Subject);

        if (user == null)
        {
            Result<User> userResult = await _userService.CreateFromGooglePayloadAsync(payload);
            if (userResult.IsSuccess)
            {
                user = userResult.Value ?? throw new InvalidOperationException("User creation returned null.");
            }
            else
            {
                throw new InvalidOperationException("User creation failed after validating Google id token.");
            }
        }

        return GenerateJwt(user);
    }

    private string GenerateJwt(User user)
    {
        var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");

        var email = user.Email ?? throw new InvalidOperationException("User email is null.");

        var claims = new[]
        {
            new Claim("userId", user.ExternalId.ToString()),
            new Claim(ClaimTypes.Email, email)
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}