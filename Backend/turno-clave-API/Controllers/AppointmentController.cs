using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Controllers
{
    [Authorize]
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            Appointment? appointment = await _appointmentService.GetByExternalIdAsync(externalId);
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
            return Ok(Appointment.ToDto(appointment));
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMyAppointments([FromQuery] DateTimeOffset fromDate, [FromQuery] DateTimeOffset toDate)
        {
            if (fromDate >= toDate)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid Date Range",
                    detail: "fromDate must be earlier than toDate.",
                    type: "/errors/InvalidDateRange",
                    instance: HttpContext.Request.Path
                );
            }

            IEnumerable<Appointment> appointments = await _appointmentService.GetMyAppointmentsAsync(fromDate, toDate);
            return Ok(appointments.Select(Appointment.ToDto));
        }


        //[HttpPut]
        //public async Task<IActionResult> Update([FromBody] UpdateAppointmentDTO dto)
        //{
        //    Result<Appointment>? result = await _appointmentService.UpdateAsync(dto);
        //    return result.ToActionResult(this, appointment => Ok(Appointment.ToDto(appointment)));
        //}

        [HttpPatch("{externalId:guid}")]
        public async Task<IActionResult> Cancel(Guid externalId)
        {
            Result<Appointment>? result = await _appointmentService.CancelAsync(externalId);
            return result.ToActionResult(this, _ => NoContent());
        }

        // ----- Public Endpoints -----
        [AllowAnonymous]
        [HttpPost("public/available-slots")]
        public async Task<IActionResult> GetAvailableSlots([FromBody] SelectionRequestDTO request)
        {
            AvailabilitySlotsResponseDTO response = await _appointmentService.GetAvailableSlotsAsync(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("public")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDTO dto)
        {
            Result<Appointment> result = await _appointmentService.CreateAsync(dto);

            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Appointment Creation Failed",
                    detail: result.Error,
                    type: "/errors/AppointmentCreationFailed",
                    instance: HttpContext.Request.Path
                );
            }

            return result.ToActionResult(this, appointment =>
            {
                AppointmentDTO dto = Appointment.ToDto(appointment);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = dto.ExternalId }, dto);
            });
        }
    }
}
