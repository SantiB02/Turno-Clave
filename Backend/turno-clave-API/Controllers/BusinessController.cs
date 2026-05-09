using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;

namespace turno_clave_API.Controllers
{
    [Route("api/businesses")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceService _serviceService;

        public BusinessController(IBusinessService businessService, ICurrentUserService currentUserService, IServiceService serviceService)
        {
            _businessService = businessService;
            _currentUserService = currentUserService;
            _serviceService = serviceService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessDTO dto)
        {
            Guid userExternalId = _currentUserService.GetExternalId();

            if (userExternalId == Guid.Empty)
            {
                return Unauthorized();
            }

            Result<BusinessDTO> result = await _businessService.CreateAsync(dto, userExternalId);
            return result.ToActionResult(this, business => CreatedAtAction(nameof(GetByExternalId), new { externalId = business.ExternalId }, business));
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            BusinessDetailDTO? business = await _businessService.GetByExternalIdAsync(externalId);

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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBusinessDTO dto)
        {
            BusinessDTO? updatedBusiness = await _businessService.UpdateAsync(dto);

            return Ok(updatedBusiness);
        }

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            BusinessDTO? business = await _businessService.DeleteAsync(externalId);

            return Ok(business);
        }
    }
}
