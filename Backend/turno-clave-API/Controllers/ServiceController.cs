using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceDTO createServiceDTO)
        {
            Result<Service> result = await _serviceService.CreateAsync(createServiceDTO);
            if (!result.IsSuccess || result.Value == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Business not found",
                    detail: result.Error,
                    type: "/errors/BusinessNotFound",
                    instance: HttpContext.Request.Path
                );
            }

            ServiceDTO dto = Service.ToDto(result.Value);
            return CreatedAtAction(nameof(GetByExternalId), new { externalId = dto.ExternalId }, dto);
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            string? userExternalIdString = User.FindFirst("userId")?.Value;
            if (userExternalIdString == null)
            {
                return Unauthorized();
            }
            Guid userExternalId = Guid.Parse(userExternalIdString);
            Result<IEnumerable<Service>> result = await _serviceService.GetByUserExternalIdAsync(userExternalId);

            IEnumerable<ServiceDTO> dtos = result.Value?.Select(Service.ToDto) ?? Enumerable.Empty<ServiceDTO>();
            return Ok(dtos);
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            Result<Service?> result = await _serviceService.GetByExternalIdAsync(externalId);
            if (!result.IsSuccess || result.Value == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Service Not Found",
                    detail: $"Service with ExternalId {externalId} not found.",
                    type: $"/errors/ServiceNotFound",
                    instance: HttpContext.Request.Path
                );
            }
            return Ok(Service.ToDto(result.Value));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateServiceDTO updateServiceDTO)
        {
            Result<Service?> result = await _serviceService.UpdateAsync(updateServiceDTO);
            if (!result.IsSuccess || result.Value == null)
            {
                return Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Service Not Found",
                        detail: $"Service with ExternalId {updateServiceDTO.ExternalId} not found.",
                        type: $"/errors/ServiceNotFound",
                        instance: HttpContext.Request.Path
                );
            }
            return Ok(Service.ToDto(result.Value));
        }

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {

            Result<Service?> result = await _serviceService.DeleteAsync(externalId);
            if (!result.IsSuccess || result.Value == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Service Not Found",
                    detail: $"Service with ExternalId {externalId} not found.",
                    type: $"/errors/ServiceNotFound",
                    instance: HttpContext.Request.Path
                );
            }
            return Ok(Service.ToDto(result.Value));
            
        }
    }
}
