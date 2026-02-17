using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs;
using turno_clave_API.Application.Interfaces;

namespace turno_clave_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessesController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessDto dto)
        {
            var business = await _businessService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = business.Id }, business);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok();
        }
    }
}
