using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                string token = await _authService.ValidateGoogle(dto.IdToken);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
