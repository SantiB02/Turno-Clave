using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using turno_clave_API.Application.Interfaces;
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

    public async Task<string> LoginWithGoogle(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            throw new ArgumentException("idToken is required", nameof(idToken));

        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

        var user = _context.Users
            .FirstOrDefault(u => u.GoogleId == payload.Subject);

        if (user == null)
        {
            await _userService.CreateFromGooglePayloadAsync(payload);
            // re-query the user after creation to ensure we have the instance
            user = _context.Users.FirstOrDefault(u => u.GoogleId == payload.Subject);
        }

        if (user == null)
            throw new InvalidOperationException("User creation failed after validating Google id token.");

        return GenerateJwt(user);
    }

    private string GenerateJwt(User user)
    {
        var key = _config["Jwt:Key"];

        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
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