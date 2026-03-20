using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Application.DTOs.Appointment
{
    public class AppointmentDTO
    {
        public Guid ExternalId { get; set; }
        public Guid BusinessExternalId { get; set; }
        public Guid ProfessionalExternalId { get; set; }
        public Guid ClientExternalId { get; set; }
        public Guid ServiceExternalId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
