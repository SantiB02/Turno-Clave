using turno_clave_API.Application.DTOs.Auth;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> ValidateGoogle(string idToken);
        Task<AuthResponseDTO> RefreshToken(string refreshToken);
        Task RevokeToken(string refreshToken);
    }
}
