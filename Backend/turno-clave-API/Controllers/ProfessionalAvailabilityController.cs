using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Common;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using turno_clave_API.Application.DTOs.ProfessionalAvailability;

namespace turno_clave_API.Controllers
{
    [Route("api/professional-availabilities")]
    [Authorize]
    [ApiController]
    public class ProfessionalAvailabilityController : ControllerBase
    {
        private readonly IProfessionalAvailabilityService _availabilityService;

        public ProfessionalAvailabilityController(IProfessionalAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProfessionalAvailabilityDTO createAvailabilityDTO)
        {
            Result<ProfessionalAvailability>? result = await _availabilityService.CreateAsync(createAvailabilityDTO);
            return result.ToActionResult(this, availability =>
            {
                ProfessionalAvailabilityDTO dto = ProfessionalAvailability.ToDto(availability);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = dto.ExternalId }, dto);
            });
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            ProfessionalAvailability? availability = await _availabilityService.GetByExternalIdAsync(externalId);
            if (availability == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Availability Not Found",
                    detail: $"Availability with ExternalId {externalId} not found.",
                    type: $"/errors/AvailabilityNotFound",
                    instance: HttpContext.Request.Path
                );
            }
            return Ok(ProfessionalAvailability.ToDto(availability));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProfessionalAvailabilityDTO updateAvailabilityDTO)
        {
            Result<ProfessionalAvailability>? result = await _availabilityService.UpdateAsync(updateAvailabilityDTO);
            return result.ToActionResult(this, availability => Ok(ProfessionalAvailability.ToDto(availability)));
        }

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            Result<ProfessionalAvailability>? result = await _availabilityService.DeleteAsync(externalId);
            return result.ToActionResult(this, _ => NoContent());
        }
    }
}
