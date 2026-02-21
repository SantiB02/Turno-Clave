using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDTO dto)
        {
            Appointment? appointment = await _appointmentService.CreateAsync(dto);

            if (appointment == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Appointment Creation Failed",
                    detail: "The appointment could not be created. Please check the provided data and try again.",
                    type: "/errors/AppointmentCreationFailed",
                    instance: HttpContext.Request.Path
                );
            }

            return CreatedAtAction(nameof(GetByExternalId), new { id = appointment.ExternalId }, appointment);
        }

        [HttpGet]
        public async Task<IActionResult> GetByExternalId(string externalId)
        {
            if (!TryParseExternalId(externalId, out var parsedExternalId, out var problem)) return problem;
            Appointment? appointment = await _appointmentService.GetByExternalId(parsedExternalId);
            if (appointment == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Appointment Not Found",
                    detail: $"Appointment with ExternalId {externalId} not found.",
                    type: $"/errors/AppointmentNotFound",
                    instance: HttpContext.Request.Path
                );
            }
            return Ok(appointment);
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
