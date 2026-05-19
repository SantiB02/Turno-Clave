using turno_clave_API.Application.DTOs.AppointmentItem;
using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Application.DTOs.Appointment
{
    public class AppointmentDTO
    {
        public Guid ExternalId { get; set; }
        public Guid BusinessExternalId { get; set; }
        public Guid ClientExternalId { get; set; }
        // Global start and end times for the appointment, stored in UTC
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public List<AppointmentItemDTO> Items { get; set; } = [];
    }
}
