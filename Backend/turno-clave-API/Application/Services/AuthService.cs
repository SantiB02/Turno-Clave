using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using turno_clave_API.Application.DTOs.Auth;
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

    public async Task<AuthResponseDTO> ValidateGoogle(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            throw new ArgumentException("idToken is required", nameof(idToken));

        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken)
            ?? throw new InvalidOperationException("Failed to validate Google id token.");

        if (string.IsNullOrWhiteSpace(payload.Subject))
            throw new InvalidOperationException("Google payload subject is missing.");

        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

        if (user == null)
        {
            Result<User> userResult = await _userService.CreateFromGooglePayloadAsync(payload);

            if (!userResult.IsSuccess || userResult.Value == null)
                throw new InvalidOperationException("User creation failed after validating Google id token.");

            user = userResult.Value;
        }

        await RevokeAllActiveTokensForUser(user.ExternalId);

        return await IssueTokens(user);
    }

    private async Task<AuthResponseDTO> IssueTokens(User user)
    {
        DateTime accessTokenExpiresAt = DateTime.UtcNow.AddHours(2);
        string accessToken = GenerateJwt(user, accessTokenExpiresAt);

        string refreshTokenValue = GenerateRefreshTokenValue();

        RefreshToken refreshToken = new RefreshToken
        {
            UserExternalId = user.ExternalId,
            Token = refreshTokenValue,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(30)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        // Load user's businesses
        List<Business> userBusinesses = await _context.UserBusinesses
            .Where(ub => ub.User.ExternalId == user.ExternalId)
            .Select(ub => ub.Business)
            .ToListAsync();

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            AccessTokenExpiresAt = accessTokenExpiresAt,
            Businesses = userBusinesses.Select(Business.ToDto).ToList()
        };
    }

    private string GenerateJwt(User user, DateTime expiresAtUtc)
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
            expires: expiresAtUtc,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponseDTO> RefreshToken(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token is required.", nameof(refreshToken));

        RefreshToken? existingToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (existingToken == null)
            throw new SecurityTokenException("Invalid refresh token.");

        if (!existingToken.IsActive)
        {
            bool withinGraceWindow =
                existingToken.RevokedAtUtc.HasValue &&
                existingToken.RevokedAtUtc.Value > DateTime.UtcNow.AddSeconds(-5) &&
                !string.IsNullOrWhiteSpace(existingToken.ReplacedByToken);

            if (withinGraceWindow)
            {
                RefreshToken? replacementToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(x => x.Token == existingToken.ReplacedByToken);

                if (replacementToken != null && replacementToken.IsActive)
                {
                    User? replacementUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.ExternalId == replacementToken.UserExternalId);

                    if (replacementUser == null)
                        throw new SecurityTokenException("User not found.");

                    DateTime graceAccessTokenExpiresAt = DateTime.UtcNow.AddHours(2);
                    string graceAccessToken = GenerateJwt(replacementUser, graceAccessTokenExpiresAt);

                    return new AuthResponseDTO
                    {
                        AccessToken = graceAccessToken,
                        RefreshToken = replacementToken.Token,
                        AccessTokenExpiresAt = graceAccessTokenExpiresAt
                    };
                }
            }

            throw new SecurityTokenException("Invalid refresh token.");
        }

        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.ExternalId == existingToken.UserExternalId);

        if (user == null)
            throw new SecurityTokenException("User not found.");

        existingToken.RevokedAtUtc = DateTime.UtcNow;

        string newRefreshTokenValue = GenerateRefreshTokenValue();
        existingToken.ReplacedByToken = newRefreshTokenValue;

        RefreshToken newRefreshToken = new RefreshToken
        {
            UserExternalId = user.ExternalId,
            Token = newRefreshTokenValue,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(30)
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        DateTime accessTokenExpiresAt = DateTime.UtcNow.AddHours(2);
        string accessToken = GenerateJwt(user, accessTokenExpiresAt);

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshTokenValue,
            AccessTokenExpiresAt = accessTokenExpiresAt
        };
    }

    public async Task RevokeToken(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token is required.", nameof(refreshToken));

        RefreshToken? existingToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (existingToken == null)
            throw new InvalidOperationException("Refresh token not found.");

        if (existingToken.RevokedAtUtc != null)
            return;

        existingToken.RevokedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    private async Task RevokeAllActiveTokensForUser(Guid userExternalId)
    {
        List<RefreshToken> activeTokens = await _context.RefreshTokens
            .Where(x =>
                x.UserExternalId == userExternalId &&
                x.RevokedAtUtc == null &&
                x.ExpiresAtUtc > DateTime.UtcNow)
            .ToListAsync();

        foreach (RefreshToken token in activeTokens)
        {
            token.RevokedAtUtc = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    private static string GenerateRefreshTokenValue()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}