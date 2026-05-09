namespace turno_clave_API.Application.DTOs.Auth
{
    public class RevokeTokenRequestDTO
    {
        public string RefreshToken { get; set; } = default!;
    }
}
