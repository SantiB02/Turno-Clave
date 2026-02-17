using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

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
            if (!TryParseExternalId(externalId, out var parsedExternalId, out var problem)) return problem;

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

        [HttpPut]
        public async Task<IActionResult> Update(string externalId, [FromBody] UpdateBusinessDto dto)
        {
            if (!TryParseExternalId(externalId, out var parsedExternalId, out var problem)) return problem;

            Business? updatedBusiness = await _businessService.UpdateAsync(parsedExternalId, dto);

            if (updatedBusiness == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Business Not Found",
                    detail: $"Business with ExternalId {externalId} not found.",
                    type: $"/errors/BusinessNotFound",
                    instance: HttpContext.Request.Path
                );
            }

            return Ok(updatedBusiness);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string externalId)
        {
            if (!TryParseExternalId(externalId, out var parsedExternalId, out var problem)) return problem;

            Business? business = await _businessService.DeleteAsync(parsedExternalId);

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

        private bool TryParseExternalId(string externalId, out Guid parsed, [NotNullWhen(false)] out IActionResult? error)
        {
            if (!Guid.TryParse(externalId, out parsed))
            {
                error = Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid ExternalId",
                    detail: $"The provided ExternalId '{externalId}' is not a valid GUID.",
                    type: "/errors/InvalidExternalId",
                    instance: HttpContext.Request.Path
                );
                return false;
            }
            error = null;
            return true;
        }
    }
}
