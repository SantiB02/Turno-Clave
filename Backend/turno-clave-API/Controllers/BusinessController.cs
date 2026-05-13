using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;

namespace turno_clave_API.Controllers
{
    [Route("api/businesses")]
    [Authorize]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly ICurrentUserService _currentUserService;

        public BusinessController(IBusinessService businessService, ICurrentUserService currentUserService)
        {
            _businessService = businessService;
            _currentUserService = currentUserService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessDTO dto)
        {
            Guid userExternalId = _currentUserService.GetExternalId();

            if (userExternalId == Guid.Empty)
            {
                return Unauthorized();
            }

            Result<MinimalBusinessDTO> result = await _businessService.CreateAsync(dto, userExternalId);
            return result.ToActionResult(this, business => CreatedAtAction(nameof(GetByExternalId), new { externalId = business.ExternalId }, business));
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            BusinessDetailDTO? business = await _businessService.GetByExternalIdAsync(externalId);

            return Ok(business);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            Guid businessExternalId = await _currentUserService.GetActiveBusinessExternalIdAsync();
            BusinessDetailDTO? business = await _businessService.GetByExternalIdAsync(businessExternalId);

            if (business == null)
                return NotFound();

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
            IEnumerable<BusinessDetailDTO> businesses = await _businessService.GetByUserExternalIdAsync(userExternalId);
            return Ok(businesses);
        }

        [HttpPut("{externalId:Guid}")]
        public async Task<IActionResult> Update(Guid externalId, [FromBody] UpdateBusinessDTO dto)
        {
            Result<MinimalBusinessDTO?> updatedBusiness = await _businessService.UpdateAsync(externalId, dto);

            if (!updatedBusiness.IsSuccess)
            {
                return updatedBusiness.Error switch
                {
                    "BUSINESS_NOT_FOUND" => NotFound(),

                    "INVALID_TIMEZONE" => Problem(
                        title: "Invalid time zone",
                        detail: "The provided time zone is invalid.",
                        statusCode: StatusCodes.Status400BadRequest
                    ),

                    _ => Problem(
                        title: "Unexpected error",
                        statusCode: StatusCodes.Status500InternalServerError
                    )
                };
            }

            return Ok(updatedBusiness.Value);
        }

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            MinimalBusinessDTO? business = await _businessService.DeleteAsync(externalId);

            return Ok(business);
        }
    }
}
