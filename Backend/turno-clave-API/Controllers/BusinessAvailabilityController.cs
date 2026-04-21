using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Application.Interfaces;

namespace turno_clave_API.Controllers
{
    [Route("api/businesses/{businessExternalId:guid}/availabilities")]
    [ApiController]
    public class BusinessAvailabilityController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessAvailabilityController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid businessExternalId)
        {
            var list = await _businessService.GetGlobalAvailabilityAsync(businessExternalId);
            return Ok(list);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Guid businessExternalId, [FromBody] CreateBusinessAvailabilityDTO dto)
        {
            var created = await _businessService.CreateGlobalAvailabilityAsync(businessExternalId, dto);
            return CreatedAtAction(nameof(Get), new { businessExternalId }, created);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BusinessAvailabilityDTO dto)
        {
            var updated = await _businessService.UpdateGlobalAvailabilityAsync(dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            bool deleted = await _businessService.DeleteGlobalAvailabilityAsync(externalId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
