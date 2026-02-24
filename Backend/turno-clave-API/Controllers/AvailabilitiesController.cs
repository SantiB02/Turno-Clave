using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Availability;
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
            try
            {
                Availability availability = await _availabilityService.CreateAsync(createAvailabilityDTO);
                AvailabilityDTO dto = Availability.ToDto(availability);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = dto.ExternalId }, dto);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Professional not found",
                    detail: ex.Message,
                    type: "/errors/ProfessionalNotFound",
                    instance: HttpContext.Request.Path
                );
            }
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
            try
            {
                Availability? availability = await _availabilityService.UpdateAsync(updateAvailabilityDTO);
                if (availability == null)
                {
                    return Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Availability Not Found",
                        detail: $"Availability with ExternalId {updateAvailabilityDTO.ExternalId} not found.",
                        type: $"/errors/AvailabilityNotFound",
                        instance: HttpContext.Request.Path
                    );
                }
                return Ok(Availability.ToDto(availability));
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Professional not found",
                    detail: ex.Message,
                    type: "/errors/ProfessionalNotFound",
                    instance: HttpContext.Request.Path
                );
            }
        }

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            try
            {
                Availability? availability = await _availabilityService.DeleteAsync(externalId);
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
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Availability Not Found",
                    detail: ex.Message,
                    type: $"/errors/AvailabilityNotFound",
                    instance: HttpContext.Request.Path
                );
            }
        }
    }
}
