using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Application.Interfaces;
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
            try
            {
                Service service = await _serviceService.CreateAsync(createServiceDTO);
                ServiceDTO dto = Service.ToDto(service);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = dto.ExternalId }, dto);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Business not found",
                    detail: ex.Message,
                    type: "/errors/BusinessNotFound",
                    instance: HttpContext.Request.Path
                );
            }
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
            IEnumerable<Service> services = await _serviceService.GetByUserExternalIdAsync(userExternalId);
            IEnumerable<ServiceDTO> dtos = services.Select(Service.ToDto);
            return Ok(dtos);
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            Service? service = await _serviceService.GetByExternalIdAsync(externalId);
            if (service == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Service Not Found",
                    detail: $"Service with ExternalId {externalId} not found.",
                    type: $"/errors/ServiceNotFound",
                    instance: HttpContext.Request.Path
                );
            }
            return Ok(Service.ToDto(service));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateServiceDTO updateServiceDTO)
        {
            try
            {
                Service? service = await _serviceService.UpdateAsync(updateServiceDTO);
                if (service == null)
                {
                    return Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Service Not Found",
                        detail: $"Service with ExternalId {updateServiceDTO.ExternalId} not found.",
                        type: $"/errors/ServiceNotFound",
                        instance: HttpContext.Request.Path
                    );
                }
                return Ok(Service.ToDto(service));
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Business not found",
                    detail: ex.Message,
                    type: "/errors/BusinessNotFound",
                    instance: HttpContext.Request.Path
                );
            }
        }

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            try
            {
                Service? service = await _serviceService.DeleteAsync(externalId);
                if (service == null)
                {
                    return Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Service Not Found",
                        detail: $"Service with ExternalId {externalId} not found.",
                        type: $"/errors/ServiceNotFound",
                        instance: HttpContext.Request.Path
                    );
                }
                return Ok(Service.ToDto(service));
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Service Not Found",
                    detail: ex.Message,
                    type: $"/errors/ServiceNotFound",
                    instance: HttpContext.Request.Path
                );
            }
        }
    }
}
