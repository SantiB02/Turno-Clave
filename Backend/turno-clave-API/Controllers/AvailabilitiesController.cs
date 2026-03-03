using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Common;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Controllers
{
    [Route("api/availabilities")]
    [ApiController]
    public class AvailabilitiesController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilitiesController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAvailabilityDTO createAvailabilityDTO)
        {
            Result<Availability>? result = await _availabilityService.CreateAsync(createAvailabilityDTO);
            return result.ToActionResult(this, availability =>
            {
                AvailabilityDTO dto = Availability.ToDto(availability);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = dto.ExternalId }, dto);
            });
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            Availability? availability = await _availabilityService.GetByExternalIdAsync(externalId);
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
            return Ok(Availability.ToDto(availability));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAvailabilityDTO updateAvailabilityDTO)
        {
            Result<Availability>? result = await _availabilityService.UpdateAsync(updateAvailabilityDTO);
            return result.ToActionResult(this, availability => Ok(Availability.ToDto(availability)));
        }

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            Result<Availability>? result = await _availabilityService.DeleteAsync(externalId);
            return result.ToActionResult(this, _ => NoContent());
        }
    }
}
