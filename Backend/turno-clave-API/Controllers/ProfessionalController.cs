using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Controllers
{
    [Route("api/professionals")]
    [Authorize]
    [ApiController]
    public class ProfessionalController : ControllerBase
    {
        private readonly IProfessionalService _professionalService;
        private readonly ICurrentUserService _currentUserService;

        public ProfessionalController(IProfessionalService professionalService, ICurrentUserService currentUserService)
        {
            _professionalService = professionalService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProfessionalDTO createProfessionalDTO)
        {
            try
            {
                Guid businessExternalId = await _currentUserService.GetActiveBusinessExternalIdAsync();
                Professional professional = await _professionalService.CreateAsync(businessExternalId, createProfessionalDTO);
                ProfessionalDTO dto = Professional.ToDto(professional);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = dto.ExternalId }, dto);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Business not found",
                    detail: ex.Message,
                    type: "/errors/ProfessionalNotFound",
                    instance: HttpContext.Request.Path
                );
            }
        }

        [HttpGet("active-business")]
        public async Task<IActionResult> GetByActiveBusiness()
        {
            Guid businessExternalId = await _currentUserService.GetActiveBusinessExternalIdAsync();
            Result<List<ProfessionalDTO>> result = await _professionalService.GetByBusinessExternalIdAsync(businessExternalId);
            if (!result.IsSuccess)
            {
                return Problem(statusCode: 500, detail: result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            Professional? professional = await _professionalService.GetByExternalIdAsync(externalId);
            if (professional == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Professional Not Found",
                    detail: $"Professional with ExternalId {externalId} not found.",
                    type: $"/errors/ProfessionalNotFound",
                    instance: HttpContext.Request.Path
                );
            }
            return Ok(Professional.ToDto(professional));
        }

        [HttpPut("{externalId:guid}")]
        public async Task<IActionResult> Update(Guid externalId, [FromBody] UpdateProfessionalDTO updateProfessionalDTO)
        {
            try
            {
                Professional? professional = await _professionalService.UpdateAsync(externalId, updateProfessionalDTO);
                if (professional == null)
                    return NotFound();
                return Ok(Professional.ToDto(professional));
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
                Guid businessExternalId = await _currentUserService.GetActiveBusinessExternalIdAsync();
                Professional? professional = await _professionalService.DeleteAsync(businessExternalId, externalId);
                if (professional == null)
                {
                    return Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Professional not found",
                        detail: $"Professional with ExternalId {externalId} not found.",
                        type: "/errors/ProfessionalNotFound",
                        instance: HttpContext.Request.Path
                    );
                }
                return Ok(Professional.ToDto(professional));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    detail: ex.Message,
                    type: "/errors/Unauthorized",
                    instance: HttpContext.Request.Path
                );
            }
        }
    }
}
