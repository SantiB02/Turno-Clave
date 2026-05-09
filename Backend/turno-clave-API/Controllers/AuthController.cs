using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Auth;
using turno_clave_API.Application.DTOs.User;
using turno_clave_API.Application.Interfaces;

namespace turno_clave_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("validate-google")]
        public async Task<IActionResult> ValidateGoogle([FromBody] GoogleAuthDTO dto)
        {
            try
            {
                AuthResponseDTO responseDto = await _authService.ValidateGoogle(dto.IdToken);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO dto)
        {
            try
            {
                var result = await _authService.RefreshToken(dto.RefreshToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequestDTO dto)
        {
            try
            {
                await _authService.RevokeToken(dto.RefreshToken);
                return Ok(new { Message = "Refresh token revoked." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
