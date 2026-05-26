using turno_clave_API.Application.DTOs.Business;

namespace turno_clave_API.Application.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime AccessTokenExpiresAt { get; set; }
        public List<MinimalBusinessDTO> Businesses { get; set; } = [];
    }
}
