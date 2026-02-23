using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Controllers
{
    [Route("api/professionals")]
    [ApiController]
    public class ProfessionalsController : ControllerBase
    {
        private readonly IProfessionalService _professionalService;

        public ProfessionalsController(IProfessionalService professionalService)
        {
            _professionalService = professionalService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProfessionalDTO createProfessionalDTO)
        {
            try
            {
                Professional professional = await _professionalService.CreateAsync(createProfessionalDTO);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = professional.ExternalId }, professional); // missing method
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

        [HttpGet]
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
            return Ok(professional);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProfessionalDTO updateProfessionalDTO)
        {
            try
            {
                Professional? professional = await _professionalService.UpdateAsync(updateProfessionalDTO);
                return Ok(professional);
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

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            try
            {
                Professional? professional = await _professionalService.DeleteAsync(externalId);
                return Ok(professional);
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
    }
}
