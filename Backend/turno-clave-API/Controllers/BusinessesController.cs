using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Controllers
{
    [Route("api/businesses")]
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
            Business business = await _businessService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByExternalId), new { externalId = business.ExternalId }, business);
        }

        [HttpGet]
        public async Task<IActionResult> GetByExternalId(string externalId)
        {
            if (!Guid.TryParse(externalId, out Guid parsedExternalId))
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid ExternalId",
                    detail: $"The provided ExternalId '{externalId}' is not a valid GUID.",
                    type: $"/errors/InvalidExternalId",
                    instance: HttpContext.Request.Path
                );
            }

            Business? business = await _businessService.GetByExternalId(parsedExternalId);

            if (business == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Business Not Found",
                    detail: $"Business with ExternalId {externalId} not found.",
                    type: $"/errors/BusinessNotFound",
                    instance: HttpContext.Request.Path
                );
            }

            return Ok(business);
        }
    }
}
