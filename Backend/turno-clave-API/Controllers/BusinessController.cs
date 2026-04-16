using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using turno_clave_API.Common;

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessDTO dto)
        {
            string? userExternalIdString = User.FindFirst("userId")?.Value;

            if (userExternalIdString == null)
            {
                return Unauthorized();
            }

            Guid userExternalId = Guid.Parse(userExternalIdString);

            Result<Business> result = await _businessService.CreateAsync(dto, userExternalId);
            return result.ToActionResult(this, business => CreatedAtAction(nameof(GetByExternalId), new { externalId = business.ExternalId }, business));
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

        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            string? userExternalIdString = User.FindFirst("userId")?.Value;
            if (userExternalIdString == null)
            {
                return Unauthorized();
            }
            Guid userExternalId = Guid.Parse(userExternalIdString);
            IEnumerable<Business> businesses = await _businessService.GetByUserExternalIdAsync(userExternalId);
            return Ok(businesses.Select(Business.ToDetailDto));
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
