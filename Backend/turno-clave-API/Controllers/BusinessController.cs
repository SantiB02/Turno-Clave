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
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessDTO dto)
        {
            try
            {
                Business business = await _businessService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = business.ExternalId }, business);
            }
            catch (ArgumentException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid TimeZone",
                    detail: ex.Message,
                    type: "/errors/InvalidTimeZone",
                    instance: HttpContext.Request.Path
                );
            }
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            Business? business = await _businessService.GetByExternalIdAsync(externalId);

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
        public async Task<IActionResult> Update([FromBody] UpdateBusinessDTO dto)
        {
            Business? updatedBusiness = await _businessService.UpdateAsync(dto);

            if (updatedBusiness == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Business Not Found",
                    detail: $"Business with ExternalId {dto.ExternalId} not found.",
                    type: $"/errors/BusinessNotFound",
                    instance: HttpContext.Request.Path
                );
            }

            return Ok(updatedBusiness);
        }

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            Business? business = await _businessService.DeleteAsync(externalId);

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
